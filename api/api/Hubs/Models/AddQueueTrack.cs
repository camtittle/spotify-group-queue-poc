﻿using Microsoft.Build.Framework;

namespace api.Hubs.Models
{
    public class AddQueueTrack
    {
        [Required]
        public string SpotifyUri { get; set; }

        [Required]
        public string Title { get; set; }

        [Required]
        public string Artist { get; set; }

        [Required]
        public long DurationMillis { get; set; }
    }
}