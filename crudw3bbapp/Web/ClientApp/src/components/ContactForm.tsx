import { Button, FormControl, TextField } from "@mui/material"
import { useState } from "react";
import { ContactApi } from "../services/ContactApi";
import { Contact, CreateContact } from "../types";

interface Props {
  selectedContact: Contact | null
  isCreate: boolean
  isUpdate: boolean
  closeModal: () => void;
}

const ContactForm = ({selectedContact, isCreate, isUpdate, closeModal}: Props) => {
  const [inputFirstName, setInputFirstName] = useState<string>(selectedContact?.firstName ?? "");
  const [inputLastName, setInputLastName] = useState<string>(selectedContact?.lastName ?? "");
  const [inputEmail, setInputEmail] = useState<string>(selectedContact?.email ?? "");
  const [inputPhoneNumber, setInputPhoneNumber] = useState<string>(selectedContact?.phoneNumber ?? "");
  const [inputCompany, setInputCompany] = useState<string>(selectedContact?.company ?? "");
  const [error, setError] = useState<string | null>(null);

  const handleInputChange = (e: React.ChangeEvent<HTMLTextAreaElement | HTMLInputElement>) => {
    if(e.target.id.includes("firstName")) {
      setInputFirstName(e.target.value)
    }

    if(e.target.id.includes("lastName")) {
      setInputLastName(e.target.value)
    }

    if(e.target.id.includes("email")) {
      setInputEmail(e.target.value)
    }

    if(e.target.id.includes("phoneNumber")) {
      setInputPhoneNumber(e.target.value)
    }

    if(e.target.id.includes("company")) {
      setInputCompany(e.target.value)
    }
  }

  const submit = () => {
    const contactApi = new ContactApi();

    const newContact: CreateContact = {
      firstName: inputFirstName,
      lastName: inputLastName,
      email: inputEmail,
      phoneNumber: inputPhoneNumber,
      company: inputCompany,
    };

    if(isCreate) {
      contactApi.createContact(newContact).then(res => {
        if(res.data.success) {
          closeModal();
        }
      }).catch(err => {
        setError(err.response.data)
      });
    }

    if(isUpdate && selectedContact) {
      contactApi.updateContact(selectedContact.id, newContact).then(res => {
        if(res.data.success) {
          closeModal();
        }
      }).catch(err => {
        setError(err.response.data)
      });
    }
  }

  return (
    <div className="selection-container">
      <FormControl variant='standard' sx={{ width: "100%", }}>
        <TextField
          id="firstNameInput"
          value={inputFirstName}
          label="First Name"
          variant="outlined"
          onChange={handleInputChange}
          required={true}
          autoFocus={true}
        />
      </FormControl>
      <FormControl variant='standard' sx={{ width: "100%", }}>
        <TextField
          id="lastNameInput"
          value={inputLastName}
          label="Last Name"
          variant="outlined"
          onChange={handleInputChange}
        />
      </FormControl>
      <FormControl variant='standard' sx={{ width: "100%" }}>
        <TextField
          id="emailInput"
          value={inputEmail}
          label="Email"
          variant="outlined"
          onChange={handleInputChange}
          type="email"
        />
      </FormControl>
      <FormControl variant='standard' sx={{ width: "100%" }}>
        <TextField
          id="phoneNumberInput"
          value={inputPhoneNumber}
          label="Phone Number"
          variant="outlined"
          onChange={handleInputChange}
        />
      </FormControl>
      <FormControl variant='standard' sx={{ width: "100%" }}>
        <TextField
          id="companyInput"
          value={inputCompany}
          label="Company"
          variant="outlined"
          onChange={handleInputChange}
        />
      </FormControl>
      <Button
        variant="contained"
        color="primary"
        size="small"
        onClick={submit}
      >
        Submit
      </Button>
      {error && <div className="error-container">
                  <div className="error-text-container">
                    <p>{error}</p>
                  </div>
                </div>}
    </div> 
  )
}

export default ContactForm;