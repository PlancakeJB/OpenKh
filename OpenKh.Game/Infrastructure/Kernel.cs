﻿using OpenKh.Common;
using OpenKh.Kh2;
using OpenKh.Kh2.Battle;
using OpenKh.Kh2.System;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace OpenKh.Game.Infrastructure
{
    public class Kernel : ILanguage
    {
        private readonly IDataContent _dataContent;

        public string Language { get; private set; }
        public BaseTable<Objentry> ObjEntries { get; private set; }
        public Dictionary<string, List<Place>> Places { get; private set; }
        public List<Ftst.Entry> Ftst { get; private set; }
        public Item Item { get; private set; }
        public List<Trsr> Trsr { get; private set; }
        public Fmlv Fmlv { get; private set; }
        public List<Kh2.Lvup.PlayableCharacter> Lvup { get; private set; }

        public Kernel(IDataContent dataContent, int languageId = 0)
        {
            Language = Constants.Languages[languageId];
            _dataContent = dataContent;

            // Load files in the same order as KH2 does
            ObjEntries = LoadFile("00objentry.bin", stream => Objentry.Read(stream));
            // 00progress
            LoadSystem("03system.bin");
            LoadBattle("00battle.bin");
            // 00common
            // 00worldpoint
            // 07localset
            // 12soundinfo
            // 14mission
            // msg/jp/fontimage
            // msg/jp/fontinfo
            Places = LoadFile($"msg/{Language}/place.bin", stream => Place.Read(stream));
            // msg/jp/sys
            // 15jigsaw
        }

        private T LoadFile<T>(string fileName, Func<Stream, T> action)
        {
            using var stream = _dataContent.FileOpen(fileName);
            return action(stream);
        }

        private void LoadSystem(string fileName)
        {
            var bar = File.OpenRead(fileName).Using(stream => Bar.Read(stream));

            bar.ForEntry("ftst", stream => Ftst = Kh2.System.Ftst.Read(stream));
            bar.ForEntry("item", stream => Item = Kh2.System.Item.Read(stream));
            bar.ForEntry("tsrs", stream => Trsr = Kh2.System.Trsr.Read(stream));
        }

        private void LoadBattle(string fileName)
        {
            var bar = File.OpenRead(fileName).Using(stream => Bar.Read(stream));

            bar.ForEntry("fmlv", stream => Fmlv = new Kh2.Battle.Fmlv(stream));
            bar.ForEntry("item", stream => Item = Kh2.System.Item.Read(stream));
            bar.ForEntry("lvup", stream => Lvup = Kh2.Lvup.Open(stream));
        }
    }
}