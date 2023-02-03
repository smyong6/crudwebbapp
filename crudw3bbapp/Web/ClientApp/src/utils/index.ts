import { GridColDef } from "@mui/x-data-grid";

export const mapToGridColumns = (objectKey: string[], width?: number): GridColDef[] => {
  const gridColumns: GridColDef[] = []

  objectKey.forEach(key => {
    if(key !== "id") {
      const newColumn: GridColDef = {field: key, headerName: key.toUpperCase(), width: width ?? 150, sortable: true};
      gridColumns.push(newColumn);
    }
  });

  return gridColumns;
}

export const createActionColumn = (name: string, func: any ): GridColDef => {
  return  {
    field: name,
    headerName: name.toUpperCase(),
    width: 80,
    sortable: false,
    renderCell: func,
    //disableClickEventBubbling: true,
  }
}