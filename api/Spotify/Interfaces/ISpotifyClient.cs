﻿using System.Collections.Generic;
using System.Threading.Tasks;
using Spotify.Models;

namespace Spotify.Interfaces
{
    public interface ISpotifyClient
    {
        Task<TrackSearchResponse> Search(string query);
        Task<AuthorizationCodeTokenResponse> GetClientToken(string code, bool isRefreshToken = false);
    }
}