import type { ProductToIndex } from "@/models";
import { ProductService } from "@/services";
import { flexRender, getCoreRowModel, useReactTable, type ColumnDef } from "@tanstack/react-table";
import { useEffect, useMemo, useState } from "react";
import { Col, Container, Form, Pagination, Row, Table } from "react-bootstrap";

export default function ProductPage() {
  const { index } = ProductService();

  const [data, setData] = useState<ProductToIndex[]>([]);
  const [_, setLoading] = useState(false);

  const [pageIndex, setPageIndex] = useState(0);
  const pageSize = 10;

  const [totalCount, setTotalCount] = useState(0);
  const [nameFilter, setNameFilter] = useState("");

  const [orderBy] = useState("CreatedAt");
  const [orderAsc] = useState(false);

  const totalPages = Math.ceil(totalCount / pageSize);

  const fetchData = async () => {
    setLoading(true);
    const res = await index({nameFilter, pageNumber: pageIndex + 1, pageSize, orderBy, orderAsc});
    if(res.data.isSuccess)
    {
      setData(res.data.data.items);
      setTotalCount(res.data.data.totalCount);
      setLoading(false);
    }
  };

  useEffect(() => {
    fetchData();
  }, [pageIndex, orderBy, orderAsc, nameFilter]);

  const columns = useMemo<ColumnDef<ProductToIndex>[]>(
    () => [
      {
        header: "Nombre",
        accessorKey: "productName",
      },
      {
        header: "Categoría",
        accessorFn: (row) => row.category.categoryName,
        id: "category",
      },
      {
        header: "Precio unitario",
        accessorKey: "unitPrice",
      },
      {
        header: "Unidades stock",
        accessorKey: "unitsInStock",
      },
      {
        header: "Unidaades en orden",
        accessorKey: "unitsOnOrder",
      },
      {
        header: "Proveedor",
        accessorFn: (row) => row.supplier.companyName,
        id: "supplier",
      },
    ], []
  );

  const table = useReactTable({
    data,
    columns,
    pageCount: totalPages,
    state: {
      pagination: {
        pageIndex,
        pageSize,
      },
    },
    manualPagination: true,
    getCoreRowModel: getCoreRowModel(),
  });

  // const handleSort = (field: string) => {
  //   setOrderAsc(orderBy === field ? !orderAsc : true);
  //   setOrderBy(field);
  // };

  return (
    <Container className="mt-4">
      <Row className="mb-3">
        <Col md={4}>
          <Form.Control
            placeholder="Buscar producto..."
            value={nameFilter}
            onChange={(e) => {
              setPageIndex(0);
              setNameFilter(e.target.value);
            }}
          />
        </Col>
      </Row>

      <Table striped bordered hover responsive>
        <thead>
          {table.getHeaderGroups().map((headerGroup) => (
            <tr key={headerGroup.id}>
              {headerGroup.headers.map((header) => (
                <th key={header.id}>
                  {header.isPlaceholder
                    ? null
                    : flexRender(
                        header.column.columnDef.header,
                        header.getContext()
                      )}
                </th>
              ))}
            </tr>
          ))}
        </thead>

        <tbody>
          {table.getRowModel().rows.map((row) => (
            <tr key={row.id}>
              {row.getVisibleCells().map((cell) => (
                <td key={cell.id}>
                  {flexRender(
                    cell.column.columnDef.cell,
                    cell.getContext()
                  )}
                </td>
              ))}
            </tr>
          ))}
        </tbody>
      </Table>

      <Pagination>
        <Pagination.Prev
          disabled={pageIndex === 0}
          onClick={() => setPageIndex((p) => p - 1)}
        />

        <Pagination.Item active>
          {pageIndex + 1} / {totalPages || 1}
        </Pagination.Item>

        <Pagination.Next
          disabled={pageIndex + 1 >= totalPages}
          onClick={() => setPageIndex((p) => p + 1)}
        />
        <span> Total: {totalCount} </span>
      </Pagination>
    </Container>
  );
}
