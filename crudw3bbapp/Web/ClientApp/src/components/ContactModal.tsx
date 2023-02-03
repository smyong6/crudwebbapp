import Modal from "react-modal";
import { Button } from "@mui/material";
import CancelIcon from '@mui/icons-material/Cancel';

interface ModalCSSProps extends React.HTMLAttributes<HTMLDivElement> {
  color?: string;
  background?: string;
  borderWidth?: string;
  borderColor?: string;
  height?: string;
  width?: string;
  closeButtonColor?: string;
  closeButtonBackground?: string;
}

interface ModalProps extends ModalCSSProps {
  hide?: boolean;
  isOpen: boolean;
  closeModal: () => void;
  title: string;
}

const modalStyles = {
  overlay: {
    zIndex: 10,
    alignItems: "center",
    justifyContent: "center",
  },
  content: {
    margin: "auto",
    border: "1px solid black",
    borderRadius: "20px",
    width: "500px",
    maxWidth: "700px",
    padding: "40px",
    alignItems: "center",
    justifyContent: "center",
  }
};

const ContactModal = ({
  closeModal,
  isOpen,
  title,
  children
}: ModalProps) => {
  return (
    <Modal
      isOpen={isOpen}
      onRequestClose={closeModal}
      style={modalStyles}
      contentLabel={title}
      ariaHideApp={false}
    >
      <div className="modal-header">
      <h1>{title}</h1>
        <Button 
          onClick={closeModal}
        >
          <CancelIcon
          fontSize="large" />
        </Button>
      </div>
      
      <div>{children}</div>
    </Modal>
  )
}

export default ContactModal;