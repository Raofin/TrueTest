'use client'
import React, { useCallback, useMemo, useState } from "react";
import { MdDelete } from "react-icons/md";
import { FaEdit } from "react-icons/fa";
import {
    Table, TableHeader, TableColumn, TableBody, TableRow, TableCell, Input, Button, Pagination, Modal, ModalContent, ModalHeader, ModalBody,
    ModalFooter,
    Tooltip,
    Select,
    SelectItem,
    Textarea,
} from "@heroui/react";
import SearchIcon from "../../components/table/search_icon/page";
import { useDisclosure } from "@heroui/react";
import { Icon } from "@iconify/react/dist/iconify.js";
import CommonModal from "@/app/components/ui/Modal/edit-delete-modal";

const columns = [
    { label: "Email", key: "email" },
    { label: "Action", key: "action" },
];

const users = [
    {
        key: "1",
        email: "alice@example.com",
    },
    {
        key: "2",
        email: "bob@example.com",
    },
    {
        key: "3",
        email: "charlie@example.com",
    },
    {
        key: "4",
        email: "david@example.com",
    },
    {
        key: "5",
        email: "eve@example.com",
    }
];
type User = {
    key: string;
    email: string;
};
export default function Component() {
    const exams: { key: string; label: string }[] = [
        { key: "a", label: "Learnathon 1.0" },
        { key: "b", label: "Learnathon 2.0" },
        { key: "c", label: "Learnathon 3.0" },
    ]
    const [filterValue, setFilterValue] = useState("");
    const rowsPerPage = 4;
    const [page, setPage] = useState(1);
    const [state, setState] = useState("");
    const [copiedEmail, setCopiedEmail] = React.useState("");
    const hasSearchFilter = Boolean(filterValue);
    const [isEditModalOpen, setIsEditModalOpen] = useState(false);
    const [isDeleteModalOpen, setIsDeleteModalOpen] = useState(false);
    const [selectedUserKey, setSelectedUserKey] = useState("");
    const handleCopyEmail = useCallback((email: string) => {
        navigator.clipboard.writeText(email).then(() => {
            setCopiedEmail(email);
            setTimeout(() => setCopiedEmail(""), 2000);
        });
    }, [setCopiedEmail]);
    const handleEdit = useCallback(() => {
        setIsEditModalOpen(false);
    }, [selectedUserKey]);

    const handleDelete = useCallback(() => {
        setIsDeleteModalOpen(false);
    }, [selectedUserKey]);
    const filteredItems = useMemo(() => {
        let filteredUsers = [...users];
        if (hasSearchFilter) {
            filteredUsers = filteredUsers.filter((user) =>
                user.email.toLowerCase().includes(filterValue.toLowerCase()),
            );
        }
        return filteredUsers;
    }, [filterValue, hasSearchFilter]);

    const pages = Math.ceil(filteredItems.length / rowsPerPage);

    const items = useMemo(() => {
        const start = (page - 1) * rowsPerPage;
        const end = start + rowsPerPage;
        return filteredItems.slice(start, end);
    }, [page, filteredItems, rowsPerPage]);

    const handleOpenEditModal = useCallback((userKey: string) => {
        setSelectedUserKey(userKey);
        setIsEditModalOpen(true);
    }, []);

    const handleOpenDeleteModal = useCallback((userKey: string) => {
        setSelectedUserKey(userKey);
        setIsDeleteModalOpen(true);
    }, []);

    const renderCell = useCallback((user: User, columnKey: React.Key) => {
        const cellValue = user[columnKey as keyof User];
        switch (columnKey) {
            case "action":
                return (
                    <div className="flex justify-center gap-4">
                        <button onClick={() => handleOpenEditModal(user.key)}><FaEdit className="text-xl" /></button>
                        <button onClick={() => handleOpenDeleteModal(user.key)}><MdDelete className="text-xl" /></button>
                    </div>);
            default:
                return (
                    <div className="flex items-center gap-2">
                        <span>{cellValue}</span>
                        <Tooltip
                            content={copiedEmail === cellValue ? "Copied!" : "Copy email"}
                        >
                            <Button
                                isIconOnly
                                variant="light"
                                size="sm"
                                onPress={() => handleCopyEmail(cellValue)}
                            >
                                <Icon
                                    icon={copiedEmail === cellValue ? "lucide:check" : "lucide:copy"}
                                    className={copiedEmail === cellValue ? "text-success" : ""}
                                    width={18}
                                />
                            </Button>
                        </Tooltip>
                    </div>
                );
        }
    }, [ copiedEmail, handleCopyEmail]);

    const onNextPage = useCallback(() => {
        if (page < pages) {
            setPage(page + 1);
        }
    }, [page, pages]);

    const onPreviousPage = useCallback(() => {
        if (page > 1) {
            setPage(page - 1);
        }
    }, [page]);

    const onSearchChange = useCallback((value?: string) => {
        if (value) {
            setFilterValue(value);
            setPage(1);
        } else {
            setFilterValue("");
        }
    }, []);

    const onClear = useCallback(() => {
        setFilterValue("");
        setPage(1);
    }, []);

    const topContent = useMemo(() => (
        <div className="mt-4 flex gap-8 w-full justify-between">
            <h2>Candidates List</h2>
            <div className="flex items-end">
                <Input
                    isClearable
                    className="w-[400px] "
                    placeholder="Search by name..."
                    startContent={<SearchIcon />}
                    value={filterValue}
                    onClear={onClear}
                    onValueChange={onSearchChange}
                />
            </div>
        </div>
    ), [filterValue, onSearchChange, onClear]);

    const bottomContent = useMemo(() => (
        <div className="py-2 px-2 flex justify-between items-center">
            <span className="w-[30%] text-small text-default-400">Page {page} out of {pages}</span>
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
                <Button  size="sm">
                    Send Invitation
                </Button>
            </div>
        </div>
    ), [page, pages, onPreviousPage, onNextPage]);
    const Mode=localStorage.getItem("theme");
    return (
        <div className={`flex flex-col ${Mode==="dark"?"bg-black":"bg-white"}`}>
            <div>
                <Select className="max-w-xs my-5 ml-4" label="Select an exam">
                    {exams.map((exam) => (
                        <SelectItem key={exam.key}>{exam.label}</SelectItem>
                    ))}
                </Select>
            </div>
            <div className="flex gap-4 mx-4">
                <Textarea label="Type candidate email to import" className=""></Textarea>
                <div className="flex flex-col gap-4">
                    <Button>Upload CSV</Button>
                    <Button>Add to list</Button>
                </div>
            </div>
            <Table
                isStriped
                className="mx-2"
                isHeaderSticky
                aria-label="Example table with custom cells, pagination"
                bottomContent={bottomContent}
                bottomContentPlacement="outside"
                classNames={{ wrapper: "" }}
                topContent={topContent}
                topContentPlacement="outside"
                selectionMode="multiple"
            >
                <TableHeader>
                    {columns.map((column) => (
                        <TableColumn
                            key={column.key}
                            align="center"
                            className="font-semibold"
                        >
                            {column.label}
                        </TableColumn>
                    ))}
                </TableHeader>
                <TableBody emptyContent="No candidate found">
                    {items.map((item) => (
                        <TableRow key={item.key}>
                            {columns.map((column) => (
                                <TableCell key={column.key} align="center">
                                    {renderCell(item, column.key)}
                                </TableCell>
                            ))}
                        </TableRow>
                    ))}
                </TableBody>
            </Table>
         
            <CommonModal
                isOpen={isEditModalOpen}
                onClose={() => setIsEditModalOpen(false)}
                title="Edit Confirmation"
                content="Do you want to edit this user record?"
                confirmButtonText="Edit"
                onConfirm={handleEdit}
            />
            <CommonModal
                isOpen={isDeleteModalOpen}
                onClose={() => setIsDeleteModalOpen(false)}
                title="Delete Confirmation"
                content="Do you want to delete this user record?"
                confirmButtonText="Delete"
                onConfirm={handleDelete}
            />

        </div>
    );
}