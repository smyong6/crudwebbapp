export type Contact = {
  id: string,
  firstName: string,
  lastName: string | null,
  email: string | null,
  phoneNumber: string | null,
  company: string | null,
}

export type CreateContact = {
  firstName: string,
  lastName: string | null,
  email: string | null,
  phoneNumber: string | null,
  company: string | null,
}

export interface BaseResult {
  success: boolean,
  error: string | null
}

export interface GetContactsResult extends BaseResult {
  contacts: Contact[]
}

export interface ContactResult extends BaseResult {
  contact: Contact
}

export interface DeleteResult extends BaseResult {
  id: string
}