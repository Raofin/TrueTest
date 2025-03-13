"use client";

import React, { useCallback, useMemo, useState, useEffect } from "react";
import { MdDelete } from "react-icons/md";
import { FaEdit } from "react-icons/fa";
import { Table, TableHeader,TableColumn, TableBody, TableRow, TableCell, Input, Button, Pagination, Modal, ModalContent, ModalHeader, ModalBody, ModalFooter,} from "@heroui/react";
import SearchIcon from "../../components/table/search_icon/page";
import { useDisclosure } from "@nextui-org/react";
import axios from "axios";
import PaginationButtons from "@/app/components/ui/pagination-button";

const columns = [
    { label: "Username", key: "username" },
    { label: "Email", key: "email" },
    { label: "Role", key: "role" },
    { label: "Created At", key: "createdAt" },
    { label: "Action", key: "action" },
];

type User = {
    username: string;
    email: string;
    role: string;
    createdAt: string;
    action?: string;
};

export default function Component() {
    const [filterValue, setFilterValue] = useState("");
    const rowsPerPage = 10;
    const [page, setPage] = useState(1);
    const { isOpen, onOpen, onClose } = useDisclosure();
    const [state, setState] = useState("");
    const hasSearchFilter = Boolean(filterValue);
    const [users, setUsers] = useState<User[]>([]);
    const [loading, setLoading] = useState(true);
    
    useEffect(() => {
        const fetchUsers = async () => {
            try {
                setLoading(true);
                const response = await axios.get(
                    `${process.env.NEXT_PUBLIC_URL}/User/Details` 
                );
                setUsers(response.data); 
            } catch (error) {
                console.error("Error fetching users:", error);
            } finally {
                setLoading(false);
            }
        };

        fetchUsers();
    },[]); 

    const filteredItems = useMemo(() => {
        let filteredUsers = [...users];
        if (hasSearchFilter) {
            filteredUsers = filteredUsers.filter((user) =>
                user.email.toLowerCase().includes(filterValue.toLowerCase())
            );
        }
        return filteredUsers;
    }, [filterValue, hasSearchFilter, users]);

    const pages = Math.ceil(filteredItems.length / rowsPerPage);

    const items = useMemo(() => {
        const start = (page - 1) * rowsPerPage;
        const end = start + rowsPerPage;
        return filteredItems.slice(start, end);
    }, [page, filteredItems, rowsPerPage]);

    const handleOpen = useCallback(
        (word: string) => {
            setState(word);
            onOpen();
        },
        [onOpen]
    );

    const renderCell = useCallback(
        (user: User, columnKey: React.Key) => {
            const cellValue = user[columnKey as keyof User];
            switch (columnKey) {
                case "action":
                    return (
                        <div className="flex gap-4 ml-16">
                            <button onClick={() => handleOpen('edit')}><FaEdit className={'text-xl'} /></button>
                            <button onClick={() => handleOpen('delete')}><MdDelete className={'text-xl'} /></button>
                        </div>
                    );
                default:
                    return cellValue;
            }
        },
        [handleOpen]
    );

    const onSearchChange = useCallback((value?: string) => {
        if (value) {
            setFilterValue(value);
            setPage(1);
        } else {
            setFilterValue("");
        }
    },[filterValue]);

    const onClear = useCallback(() => {
        setFilterValue("");
        setPage(1);
    },[page]);

    const topContent = useMemo(() => (
            <div className="flex gap-8 mt-8 w-full justify-between">
                <h2>Users List</h2>
                <div className="flex items-end">
                    <Input
                        isClearable
                        className="w-[400px]"
                        placeholder="Search by name..."
                        startContent={<SearchIcon />}
                        value={filterValue}
                        onClear={onClear}
                        onValueChange={onSearchChange}
                    />
                </div>
            </div>
        ),
        [filterValue, onSearchChange, onClear]
    );

    const bottomContent = useMemo(
        () => (
            <div className="py-4 px-2 flex justify-between items-center">
                <span className="w-[30%] text-small text-default-400">
                    Page {page} out of {pages}
                </span>
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
                <PaginationButtons
                      currentIndex={page}
                      totalItems={pages}
                      onPrevious={() => setPage(page - 1)}
                      onNext={() => setPage(page + 1)}/>
                    <Button color="primary" size="sm">
                        Export
                    </Button>
                </div>
            </div>
        ),
        [page, pages]
    );

    return (
        <>
            <div className="flex flex-col h-screen">
                <h2 className="text-2xl font-bold my-4 text-center flex justify-center">
                    Users Management
                </h2>
                {loading ? (
                    <div className="flex justify-center items-center h-full">
                        <p>Loading...</p> 
                    </div>
                ) : (
                    <Table
                        isStriped
                        isHeaderSticky
                        aria-label="Example table with custom cells, pagination"
                        bottomContent={bottomContent}
                        bottomContentPlacement="outside"
                        classNames={{ wrapper: "min-h-[70vh] max-h-[80vh] overflow-y-auto" }}
                        topContent={topContent}
                        topContentPlacement="outside"
                    >
                        <TableHeader>
                            {columns.map((column) => (
                                <TableColumn
                                    key={column.key}
                                    align={"center"}
                                    className={"font-semibold"}
                                >
                                    {column.label}
                                </TableColumn>
                            ))}
                        </TableHeader>
                        <TableBody emptyContent="No user found" className={items.length === 0 ? "min-h-[70vh]" : "min-h-[auto]"}>
                            {items.map((item) => (
                                <TableRow key={item.email}>
                                    {columns.map((column) => (
                                        <TableCell key={column.key}>{renderCell(item, column.key)}</TableCell>
                                    ))}
                                </TableRow>
                            ))}
                        </TableBody>
                    </Table>
                )}
            </div>

            <Modal isOpen={isOpen} onClose={onClose}>
                <ModalContent>
                    <ModalHeader className="flex flex-col gap-1">
                        {state === "edit" ? "Edit Confirmation" : "Delete Confirmation"}
                    </ModalHeader>
                    <ModalBody>
                        <p>
                            {`Do you want to ${state === "edit" ? "edit this user record" : "delete this user record"}?`}
                        </p>
                    </ModalBody>
                    <ModalFooter>
                        <Button color="primary" variant="light" onPress={onClose}>
                            Close
                        </Button>
                        <Button color="primary">
                            {state === "edit" ? "Edit" : "Delete"}
                        </Button>
                    </ModalFooter>
                </ModalContent>
            </Modal>
        </>
    );
}