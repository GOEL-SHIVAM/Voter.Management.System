export interface Election {
  electionId: number;
  electionCode: string;
  status: ElectionStatus;
  winner: string;
  totalVotes: number;
  partyCount: number;
}

export enum ElectionStatus {
  Registered = 'Registered',
  Started = 'Started',
  Ended = 'Ended',
}

export interface CreateElectionRequest {
  electionCode: string;
}

export interface AddPartyToElectionRequest {
  partyName: string;
}

export interface CastVoteRequest {
  electionCode: string;
  partyName: string;
}

export interface ElectionResult {
  electionCode: string;
  winner: string;
  partyResults: PartyVoteCount[];
}

export interface PartyVoteCount {
  partyName: string;
  votes: number;
  percentage: number;
}
