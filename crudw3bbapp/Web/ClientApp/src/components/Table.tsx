import React, { Dispatch, SetStateAction} from 'react';
import { DataGrid, GridColDef} from '@mui/x-data-grid';
import { Contact } from '../types';

interface Props {
  rows: Contact[] | [];
  columns: GridColDef[];
  pageSize: number;
  setSelection: Dispatch<SetStateAction<Contact | null>>;
}

const Table = ({rows, columns, pageSize, setSelection}: Props) => {
  return (
    <div style={{ height: "400px", width: "950px" }}>
      <DataGrid
        rows={rows}
        columns={columns}
        pageSize={pageSize}
        rowsPerPageOptions={[pageSize]}
        onRowClick={(e) => setSelection(e.row)}
      />
    </div>
  )
}

export default Table;