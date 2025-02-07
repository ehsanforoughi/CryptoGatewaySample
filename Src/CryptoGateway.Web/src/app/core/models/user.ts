import { Role } from './role';

export class User {
  id!: number;
  image!: string;
  username!: string;
  password!: string;
  firstName!: string;
  lastName!: string;
  fullName!: string;
  role!: Role;
  token!: string;
}
