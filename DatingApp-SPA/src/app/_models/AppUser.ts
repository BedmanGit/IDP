import { Photo } from './photo';

export interface AppUser {
  id: number;
  userName: string;
  knownAs: string;
  age: number;
  created: Date;
  lastActive: Date;
  photoUrl: string;
  city: string;
  country: string;
  interests?: string;
  introduction: string;
  lookingFor?: string;
  photos?: Photo[];
}
