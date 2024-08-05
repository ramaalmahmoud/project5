using Project5_election_website.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Project5_election_website.Models
{
    public class ListViewModel
    {
        public List<localList> LocalLists { get; set; }
        public Dictionary<int, List<LocalCandidate>> CandidatesByList { get; set; }

        public List<PartyList> PartyLists { get; set; }
        public Dictionary<int, List<PartyCandidate>> PartyCandidatesByList { get; set; }


    }


}