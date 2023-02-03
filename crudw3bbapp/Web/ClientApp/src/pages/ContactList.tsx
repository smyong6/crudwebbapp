import { Button } from "@mui/material";
import EditIcon from '@mui/icons-material/Edit';
import DeleteIcon from '@mui/icons-material/Delete';
import { GridColDef } from "@mui/x-data-grid";
import { useEffect, useState } from "react";
import Table from "../components/Table"
import { ContactApi } from "../services/ContactApi";
import { Contact } from "../types";
import { createActionColumn, mapToGridColumns } from "../utils";
import ContactModal from "../components/ContactModal";
import ContactForm from "../components/ContactForm";
import ConfirmModal from "../components/ConfirmModal";

const ContactList = () => {
  const [contacts, setContacts] = useState<Contact[] | []>([]);
  const [columns, setColumns] = useState<GridColDef[] | []>([]);
  const [selectedContact, setSelectedContact] = useState<Contact | null>(null);
  const [isCreateContact, setIsCreateContact] = useState<boolean>(false);
  const [isUpdateContact, setIsUpdateContact] = useState<boolean>(false);
  const [isDeleteContact, setIsDeleteContact] = useState<boolean>(false);
  const [getContacts, setGetContacts] = useState<boolean>(true);
  const [showModal, setShowModal] = useState<boolean>(false);
  const [error, setError] = useState<string | null>(null);
  
  const contactApi = new ContactApi();
  
  useEffect(() => {
    setError(null);
    contactApi.getContacts().then((res) => {
      if(res.data.success) {
        setContacts(res.data.contacts);
        setGetContacts(false);
        
        setColumns([]);
        formatColumns();
      }
    })
    .catch(err => {
      setError(err.response?.data);
    });
  }, [getContacts])

  const renderContactModal = () => {
    setShowModal(true)
  }

  const renderEditButton = () => {
    return (
      <Button
          variant="contained"
          color="primary"
          size="small"
          onClick={() => {
            renderContactModal();
            setIsUpdateContact(true);
          }}
      >
        <EditIcon/>
      </Button>
    )
  }

  const renderDeleteButton = () => {
    return (
      <Button
          variant="contained"
          color="primary"
          size="small"
          onClick={() => {
              renderContactModal();
              setIsDeleteContact(true);
          }}
      >
        <DeleteIcon/>
      </Button>
    )
  }

  const deleteItem = () => {
    if(selectedContact) contactApi.deleteContact(selectedContact.id).then(res => {

      if(res.data.success) {
        if(contacts.length === 1) setContacts([]);

        closeModal();
      }
    }).catch(err => {
      setError(err.response.data)
    });
  }

  const formatColumns = () => {
    let columns = mapToGridColumns(Object.keys(contacts[0]));

    const editColumn = createActionColumn("edit", renderEditButton);
    const deleteColumn = createActionColumn("delete", renderDeleteButton);

    columns.push(editColumn, deleteColumn);

    setColumns(columns);
  };

  const closeModal = () => {
    setShowModal(false);
    setGetContacts(true);
    setIsCreateContact(false);
    setIsUpdateContact(false);
    setIsDeleteContact(false);
  }

  return (
    <div className="list-container">
      <Button
        variant="contained"
        color="primary"
        size="small"
        onClick={() => {
          renderContactModal();
          setIsCreateContact(true);
        }}
      >
        Create Contact
      </Button>
      {showModal && isCreateContact && <ContactModal
                                          isOpen={showModal}
                                          closeModal={() => closeModal()}
                                          title={"Create Contact"}
                                          width={"250px"}
                                          height={"250px"}
                                        >
                                          <ContactForm selectedContact={selectedContact} isCreate={true} isUpdate={false} closeModal={() => closeModal()}/>
                                        </ContactModal>}
      {showModal && isUpdateContact && selectedContact && <ContactModal
                                                              isOpen={showModal}
                                                              closeModal={() => closeModal()}
                                                              title="Update Existing Contact"
                                                            >
                                                              <ContactForm selectedContact={selectedContact} isCreate={false} isUpdate={true} closeModal={() => closeModal()}/>
                                                            </ContactModal>}
      {showModal && isDeleteContact && selectedContact && <ConfirmModal
                                                              isOpen={showModal}
                                                              closeModal={() => closeModal()}
                                                              deleteItem={() => deleteItem()}
                                                              title="Delete Contact"
                                                            >
                                                            </ConfirmModal>}
      <Table rows={contacts} columns={columns} pageSize={20} setSelection={setSelectedContact}/>
      {error && <div className="error-container">
                  <div className="error-text-container">
                    <p>{error}</p>
                  </div>
                </div>}
    </div>
  )
}

export default ContactList;