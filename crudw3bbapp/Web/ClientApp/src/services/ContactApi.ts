import { ContactResult, CreateContact, DeleteResult, GetContactsResult } from "../types";
import { BaseApi } from "./BaseApi";

export class ContactApi extends BaseApi {

  constructor() {
    super("https://localhost:5000")
  }

  public getContacts() {
    return this.instance.get<GetContactsResult>("/contact");
  }

  public createContact(data: CreateContact) {
    return this.instance.post<ContactResult>("/contact/create", data);
  }

  public updateContact(id: string, data: CreateContact) {
    return this.instance.put<ContactResult>(`/contact/update/${id}`, data);
  }

  public deleteContact(id: string) {
    return this.instance.delete<DeleteResult>(`/contact/delete/${id}`);
  }
}