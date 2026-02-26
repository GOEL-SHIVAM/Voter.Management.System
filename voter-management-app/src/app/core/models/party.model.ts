export interface Party {
  partyId: number;
  partyName: string;
}

export interface CreatePartyRequest {
  partyName: string;
  password: string;
}
