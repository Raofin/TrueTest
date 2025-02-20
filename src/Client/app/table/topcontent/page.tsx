import React from "react";
import { Icon } from "@iconify/react";
import { MdDelete } from "react-icons/md";
import { FaEdit } from "react-icons/fa";
import {
    Table, TableHeader, TableColumn, TableBody, TableRow, TableCell, Input, Button, Pagination,
    SortDescriptor, Modal, ModalContent, ModalHeader, ModalBody, ModalFooter, useDisclosure,
} from "@heroui/react";
import SearchIcon from "@/app/table/search_icon/page";

interface Column {
    name: string;
    uid: string;
    sortable?: boolean;
}

interface User {
    [key: string]: any;
}

interface CommonTableProps {
    columns: Column[];
    users: User[];
    filterKey: string;
    tableTitle: string;
    onEdit: (item: User) => void;
    onDelete: (item: User) => void;
    itemKey?: string;
}

export const CommonTable: React.FC<CommonTableProps> = ({ columns, users, filterKey, tableTitle, onEdit, onDelete, itemKey = "id" }) => {
    const [filterValue, setFilterValue] = React.useState("");
    const [rowsPerPage, setRowsPerPage] = React.useState(5);
    const [sortDescriptor, setSortDescriptor] = React.useState<SortDescriptor>({
        column: columns.find(c => c.sortable)?.uid || columns[0].uid,
        direction: "ascending",
    });
    const [page, setPage] = React.useState(1);
    const { isOpen, onOpen, onClose } = useDisclosure();
    const [modalAction, setModalAction] = React.useState<"edit" | "delete" | null>(null);
    const [currentItem, setCurrentItem] = React.useState<User | null>(null);

    const hasSearchFilter = Boolean(filterValue);

    const headerColumns = React.useMemo(() => {
        return columns.filter((column) => column.uid !== "action");
    }, [columns]);

    const filteredItems = React.useMemo(() => {
        let filteredUsers = [...users];

        if (hasSearchFilter) {
            filteredUsers = filteredUsers.filter((user) =>
                user[filterKey].toLowerCase().includes(filterValue.toLowerCase()),
            );
        }
        return filteredUsers;
    }, [filterValue, hasSearchFilter, users, filterKey]);

    const pages = Math.ceil(filteredItems.length / rowsPerPage);
    const items = React.useMemo(() => {
        const start = (page - 1) * rowsPerPage;
        const end = start + rowsPerPage;
        return filteredItems.slice(start, end);
    }, [page, filteredItems, rowsPerPage]);

    const sortedItems = React.useMemo(() => {
        const column = sortDescriptor.column as keyof User;
        return [...items].sort((a, b) => {
            const first = a[column];
            const second = b[column];
            const cmp = first < second ? -1 : first > second ? 1 : 0;
            return sortDescriptor.direction === "descending" ? -cmp : cmp;
        });
    }, [sortDescriptor, items]);

    const handleOpenModal = (action: "edit" | "delete", item: User) => {
        setModalAction(action);
        setCurrentItem(item);
        onOpen();
    };

    const handleModalConfirm = () => {
        if (modalAction && currentItem) {
            if (modalAction === "edit") {
                onEdit(currentItem);
            } else if (modalAction === "delete") {
                onDelete(currentItem);
            }
            onClose();
            setCurrentItem(null);
            setModalAction(null);
        }
    };

    const renderCell = React.useCallback((user: User, columnKey: React.Key) => {
        const cellValue = user[columnKey as keyof User];

        switch (columnKey) {
            case "action":
                return (
                    <div className='flex gap-4 ml-28'>
                        <button onClick={() => handleOpenModal('edit', user)}><FaEdit className={'text-xl'} /></button>
                        <button onClick={() => handleOpenModal('delete', user)}><MdDelete className={'text-xl'} /></button>
                    </div>
                );
            default:
                return cellValue;
        }
    }, [handleOpenModal, onEdit, onDelete]);

    const onNextPage = React.useCallback(() => {
        if (page < pages) {
            setPage(page + 1);
        }
    }, [page, pages]);

    const onPreviousPage = React.useCallback(() => {
        if (page > 1) {
            setPage(page - 1);
        }
    }, [page]);

    const onRowsPerPageChange = React.useCallback((e: React.ChangeEvent<HTMLSelectElement>) => {
        setRowsPerPage(Number(e.target.value));
        setPage(1);
    }, []);

    const onSearchChange = React.useCallback((value?: string) => {
        if (value) {
            setFilterValue(value);
            setPage(1);
        } else {
            setFilterValue("");
        }
    }, []);

    const onClear = React.useCallback(() => {
        setFilterValue("");
        setPage(1);
    }, []);

    const topContent = React.useMemo(() => (
        <div className="flex flex-col gap-4 mt-8 w-full">
            <h3 className="text-2xl text-center font-bold">{tableTitle}</h3>
            <div className="flex justify-between gap-3 items-end">
                <Input
                    isClearable
                    className="w-full sm:max-w-[30%]"
                    placeholder="Search..."
                    startContent={<SearchIcon />}
                    value={filterValue}
                    onClear={onClear}
                    onValueChange={onSearchChange}
                />
                <Button color="primary" startContent={<Icon icon="solar:add-circle-bold" width={20} />}>
                    Add
                </Button>
            </div>
            <div className="flex justify-between items-center">
                <span className="text-default-400 text-small">Total {users.length} items</span>
                <label className="flex items-center text-default-400 text-small">
                    Rows per page:
                    <select
                        className="bg-transparent outline-none text-default-400 text-small"
                        onChange={onRowsPerPageChange}
                    >
                        <option value="5">5</option>
                        <option value="10">10</option>
                        <option value="15">15</option>
                    </select>
                </label>
            </div>
        </div>
    ), [filterValue, onSearchChange, onRowsPerPageChange, onClear, users.length, tableTitle]);

    const bottomContent = React.useMemo(() => (
        <div className="py-2 px-2 flex justify-between items-center">
            <span className="w-[30%] text-small text-default-400"></span>
            <Pagination
                isCompact
                showControls
                showShadow
                color="primary"
                page={page}
                total={pages}
                onChange={setPage}
            />
            <div className="hidden sm:flex w-[30%] justify-end gap-2">
                <Button isDisabled={pages === 1} size="sm" variant="flat" onPress={onPreviousPage}>
                    Previous
                </Button>
                <Button isDisabled={pages === 1} size="sm" variant="flat" onPress={onNextPage}>
                    Next
                </Button>
            </div>
        </div>
    ), [page, pages, onPreviousPage, onNextPage]);

    return (
        <>
            <Table
                className="px-10 mx-2"
                isHeaderSticky
                aria-label="Table"
                bottomContent={bottomContent}
                bottomContentPlacement="outside"
                classNames={{
                    wrapper: "max-h-[382px]",
                }}
                sortDescriptor={sortDescriptor}
                topContent={topContent}
                topContentPlacement="outside"
                onSortChange={setSortDescriptor}
            >
                <TableHeader columns={headerColumns}>
                    {(column) => (
                        <TableColumn
                            key={column.uid}
                            align={"center"}
                            className={"font-semibold"}
                            allowsSorting={column.sortable}
                        >
                            {column.name}
                        </TableColumn>
                    )}
                </TableHeader>
                <TableBody emptyContent={"No items found"} items={sortedItems}>
                    {(item) => (
                        <TableRow key={item[itemKey]}>
                            {(columnKey) => <TableCell>{renderCell(item, columnKey)}</TableCell>}
                        </TableRow>
                    )}
                </TableBody>
            </Table>

            <Modal isOpen={isOpen} onClose={onClose}>
                <ModalContent>
                    {(onClose) => (
                        <>
                            <ModalHeader className="flex flex-col gap-1">{modalAction === 'edit' ? "Edit Confirmation" : "Delete Confirmation"}</ModalHeader>
                            <ModalBody>
                                <p>
                                    {`Do you want to ${modalAction === 'edit' ? "edit this record" : "delete this record"}?`}
                                </p>
                            </ModalBody>
                            <ModalFooter>
                                <Button color="primary" variant="light" onPress={onClose}>Close</Button>
                                <Button color="danger" onPress={handleModalConfirm}>{modalAction === 'edit' ? "Edit" : "Delete"}</Button>
                            </ModalFooter>
                        </>
                    )}
                </ModalContent>
            </Modal>
        </>
    );
};