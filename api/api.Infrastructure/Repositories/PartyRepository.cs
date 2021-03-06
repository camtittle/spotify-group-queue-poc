﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Api.Domain.Entities;
using Api.Domain.Interfaces.Repositories;
using Api.Infrastructure.DbContexts;
using Microsoft.EntityFrameworkCore;

namespace Api.Infrastructure.Repositories
{
    public class PartyRepository : IPartyRepository
    {
        private readonly SpotifyAppContext _context;

        public PartyRepository(SpotifyAppContext context)
        {
            _context = context;
        }

        public async Task Add(Party party)
        {
            if (party == null)
            {
                throw new ArgumentNullException();
            }

            _context.Parties.Add(party);
            await _context.SaveChangesAsync();
        }

        public async Task<List<Party>> GetAll()
        {
            return await _context.Parties
                .Include(p => p.Members)
                .Include(p => p.PendingMembers)
                .Include(p => p.Owner)
                .ToListAsync();
        }

        public async Task<Party> Get(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                throw new ArgumentNullException();
            }

            return await _context.Parties.Where(p => p.Id == id)
                .Include(p => p.Owner)
                .Include(p => p.Members)
                .Include(p => p.PendingMembers)
                .FirstOrDefaultAsync();
        }

        public async Task<Party> GetByName(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentNullException();
            }

            return await _context.Parties.Where(p => p.Name == name)
                .Include(p => p.Owner)
                .Include(p => p.Members)
                .Include(p => p.PendingMembers)
                .FirstOrDefaultAsync();
        }

        public async Task Delete(Party party)
        {
            if (party == null)
            {
                throw new ArgumentNullException();
            }

            _context.Parties.Remove(party);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> Exists(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                throw new ArgumentNullException();
            }

            return await _context.Parties.AnyAsync(e => e.Id == id);
        }

        public async Task<Party> GetWithAllProperties(Party party)
        {
            if (party == null)
            {
                throw new ArgumentNullException();
            }

            return await _context.Parties.Where(p => p.Id == party.Id)
                .Include(p => p.Members).ThenInclude(u => u.CurrentParty)
                .Include(p => p.PendingMembers).ThenInclude(u => u.PendingParty)
                .Include(p => p.Owner).ThenInclude(u => u.OwnedParty)
                .Include(p => p.QueueItems)
                .Include(p => p.CurrentTrack)
                .FirstOrDefaultAsync();
        }

        public async Task Update(Party party)
        {
            if (party == null)
            {
                throw new ArgumentNullException();
            }

            _context.Parties.Update(party);
            await _context.SaveChangesAsync();
        }
    }
}