﻿using System;
using EnsureThat;

namespace CodeGolf.Domain
{
    public class HoleInstance
    {
        public HoleInstance(Guid holeId, DateTime start, DateTime end)
        {
            this.HoleId = EnsureArg.IsNotEmpty(holeId, nameof(holeId));
            this.Start = EnsureArg.IsNotDefault(start, nameof(start));
            this.End = EnsureArg.IsNotDefault(end, nameof(end));
        }

        public Guid HoleId { get; private set; }

        public DateTime Start { get; private set; }

        public DateTime End { get; private set; }
    }
}