export interface Voter {
  voterId: number;
  aadhar: string;
  name: string;
  birthDate: Date;
  age: number;
}

export interface CreateVoterRequest {
  aadhar: string;
  password: string;
  name: string;
  birthDate: Date;
}

export interface UpdateVoterRequest {
  name?: string;
  password?: string;
}

export interface VoteHistory {
  electionCode: string;
  partyName: string;
  votedAt: Date;
}
