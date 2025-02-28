"use client"
import React, {SVGProps} from "react";
import {
    Table,
    TableHeader,
    TableColumn,
    TableBody,
    TableRow,
    TableCell,
    Input,
    Button,
    DropdownTrigger,
    Dropdown,
    DropdownMenu,
    DropdownItem,
    Chip,
    User,
    Pagination,
    Selection,
    ChipProps,
    SortDescriptor,
} from "@heroui/react";

export type IconSvgProps = SVGProps<SVGSVGElement> & {
    size?: number;
};

function capitalize(s: string) {
    return s ? s.charAt(0).toUpperCase() + s.slice(1).toLowerCase() : "";
}
const SearchIcon = (props: IconSvgProps) => {
    return (
        <svg
            aria-hidden="true"
            fill="none"
            focusable="false"
            height="1em"
            role="presentation"
            viewBox="0 0 24 24"
            width="1em"
            {...props}
        >
            <path
                d="M11.5 21C16.7467 21 21 16.7467 21 11.5C21 6.25329 16.7467 2 11.5 2C6.25329 2 2 6.25329 2 11.5C2 16.7467 6.25329 21 11.5 21Z"
                stroke="currentColor"
                strokeLinecap="round"
                strokeLinejoin="round"
                strokeWidth="2"
            />
            <path
                d="M22 22L20 20"
                stroke="currentColor"
                strokeLinecap="round"
                strokeLinejoin="round"
                strokeWidth="2"
            />
        </svg>
    );
};

const ChevronDownIcon = ({strokeWidth = 1.5, ...otherProps}: IconSvgProps) => {
    return (
        <svg
            aria-hidden="true"
            fill="none"
            focusable="false"
            height="1em"
            role="presentation"
            viewBox="0 0 24 24"
            width="1em"
            {...otherProps}
        >
            <path
                d="m19.92 8.95-6.52 6.52c-.77.77-2.03.77-2.8 0L4.08 8.95"
                stroke="currentColor"
                strokeLinecap="round"
                strokeLinejoin="round"
                strokeMiterlimit={10}
                strokeWidth={strokeWidth}
            />
        </svg>
    );
};

const columns = [
    {name: "ID", uid: "id"},
    {name: "NAME", uid: "name"},
    {name: "EMAIL", uid: "email"},
    {name: "ROLE", uid: "role",sortable: true},
    {name: "DATE", uid: "date",sortable: true},
    {name: "STATUS", uid: "status", sortable: true},
];

const statusOptions = [
    {name: "Upcoming", uid: "upcoming"},
    {name: "Finished", uid: "finished"},
    {name: "Running", uid: "running"},
];
const users = [
    {
        "id": 1,
        "name": "Alice Johnson",
        "email": "alice.johnson@example.com",
        "role": "Project Manager",
        "date": "20-02-2025",
        "status": "finished"
    },
    {
        "id": 2,
        "name": "Tony Reichert",
        "email": "tony.reichert@example.com",
        "role": "Software Engineer",
        "date": "25-02-2025",
        "status": "finished"
    },
    {
        "id": 3,
        "name": "John Doe",
        "email": "john.doe@example.com",
        "role": "UI/UX Designer",
        "date": "22-02-2025",
        "status": "finished"
    },
    {
        "id": 4,
        "name": "Emma Brown",
        "email": "emma.brown@example.com",
        "role": "QA Engineer",
        "date": "18-02-2025",
        "status": "running"
    },
    {
        "id": 5,
        "name": "David Wilson",
        "email": "david.wilson@example.com",
        "role": "Backend Developer",
        "date": "27-02-2025",
        "status": "finished"
    },
    {
        "id": 6,
        "name": "Sophia Martinez",
        "email": "sophia.martinez@example.com",
        "role": "Frontend Developer",
        "date": "15-02-2025",
        "status": "running"
    },
    {
        "id": 7,
        "name": "James Anderson",
        "email": "james.anderson@example.com",
        "role": "Software Engineer",
        "date": "10-02-2025",
        "status": "finished"
    },
    {
        "id": 8,
        "name": "Olivia Taylor",
        "email": "olivia.taylor@example.com",
        "role": "DevOps Engineer",
        "date": "05-02-2025",
        "status": "running"
    },
    {
        "id": 9,
        "name": "Michael Scott",
        "email": "michael.scott@example.com",
        "role": "Product Owner",
        "date": "28-02-2025",
        "status": "finished"
    },
    {
        "id": 10,
        "name": "William Brown",
        "email": "william.brown@example.com",
        "role": "Security Analyst",
        "date": "21-02-2025",
        "status": "finished"
    },
    {
        "id": 11,
        "name": "Charlotte Green",
        "email": "charlotte.green@example.com",
        "role": "Database Administrator",
        "date": "12-02-2025",
        "status": "finished"
    },
    {
        "id": 12,
        "name": "Daniel White",
        "email": "daniel.white@example.com",
        "role": "Cloud Engineer",
        "date": "14-02-2025",
        "status": "running"
    },
    {
        "id": 13,
        "name": "Amelia Harris",
        "email": "amelia.harris@example.com",
        "role": "Software Architect",
        "date": "16-02-2025",
        "status": "running"
    },
    {
        "id": 14,
        "name": "Benjamin Lewis",
        "email": "benjamin.lewis@example.com",
        "role": "Full Stack Developer",
        "date": "24-02-2025",
        "status": "finished"
    },
    {
        "id": 15,
        "name": "Mia Robinson",
        "email": "mia.robinson@example.com",
        "role": "Business Analyst",
        "date": "19-02-2025",
        "status": "finished"
    }
];
const statusColorMap: Record<string, ChipProps["color"]> = {
    active: "success",
    paused: "danger",
    vacation: "warning",
};
const INITIAL_VISIBLE_COLUMNS = ["id","name","email", "role", "date","status"];
type User = (typeof users)[0];
export default function Component() {
    const [filterValue, setFilterValue] = React.useState("");
    const [selectedKeys, setSelectedKeys] = React.useState<Selection>(new Set([]));
    const [visibleColumns, setVisibleColumns] = React.useState<Selection>(
        new Set(INITIAL_VISIBLE_COLUMNS),);
    const [statusFilter, setStatusFilter] = React.useState<Selection>("all");
    const [rowsPerPage, setRowsPerPage] = React.useState(5);
    const [sortDescriptor, setSortDescriptor] = React.useState<SortDescriptor>({
        column:"date",
        direction: "ascending",
    });
    const [page, setPage] = React.useState(1);
    const hasSearchFilter = Boolean(filterValue);
    const headerColumns = React.useMemo(() => {
        if (visibleColumns === "all"){ setVisibleColumns("all"); return columns;}

        return columns.filter((column) => Array.from(visibleColumns).includes(column.uid));
    }, [visibleColumns]);
    const filteredItems = React.useMemo(() => {
        let filteredUsers = [...users];

        if (hasSearchFilter) {
            filteredUsers = filteredUsers.filter((user) =>
                user.name.toLowerCase().includes(filterValue.toLowerCase()),
            );
        }
        if (statusFilter !== "all" && Array.from(statusFilter).length !== statusOptions.length) {
            filteredUsers = filteredUsers.filter((user) =>
                Array.from(statusFilter).includes(user.status),
            );
        }

        return filteredUsers;
    }, [ filterValue, statusFilter,hasSearchFilter]);

    const pages = Math.ceil(filteredItems.length / rowsPerPage);

    const items = React.useMemo(() => {
        const start = (page - 1) * rowsPerPage;
        const end = start + rowsPerPage;

        return filteredItems.slice(start, end);
    }, [page, filteredItems, rowsPerPage]);

    const sortedItems = React.useMemo(() => {
        return [...items].sort((a: User, b: User) => {
            const first = a[sortDescriptor.column as keyof User] as number;
            const second = b[sortDescriptor.column as keyof User] as number;
            const cmp = first < second ? -1 : first > second ? 1 : 0;

            return sortDescriptor.direction === "descending" ? -cmp : cmp;
        });
    }, [sortDescriptor, items]);

    const renderCell = React.useCallback((user: User, columnKey: React.Key) => {
        const cellValue = user[columnKey as keyof User];
        switch (columnKey) {
            case "name":
                return (
                    <User name={cellValue}>
                        {user.email}
                    </User>
                );
            case "role":
                return (
                    <div className="flex flex-col">
                        <p className="text-bold text-small capitalize">{cellValue}</p>
                    </div>
                );
            case "status":
                return (
                    <Chip className="capitalize" color={statusColorMap[user.status]} size="sm" variant="flat">
                        {cellValue}
                    </Chip>
                );
            default:
                return cellValue;
        }
    }, []);

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

    const topContent = React.useMemo(() => {
        return (
            <div className="flex flex-col gap-4 mt-12">
                <div className="flex justify-between gap-3 items-end">
                    <Input
                        isClearable
                        className="w-full sm:max-w-[30%]"
                        placeholder="Search by name..."
                        startContent={<SearchIcon />}
                        value={filterValue}
                        onClear={() => onClear()}
                        onValueChange={onSearchChange}
                    />
                    <div className="flex gap-3">
                        <Dropdown>
                            <DropdownTrigger className="hidden sm:flex">
                                <Button endContent={<ChevronDownIcon className="text-small" />} variant="flat">
                                    Status
                                </Button>
                            </DropdownTrigger>
                            <DropdownMenu
                                disallowEmptySelection
                                aria-label="Table Columns"
                                closeOnSelect={false}
                                selectedKeys={statusFilter}
                                selectionMode="multiple"
                                onSelectionChange={setStatusFilter}
                            >
                                {statusOptions.map((status) => (
                                    <DropdownItem key={status.uid} className="capitalize">
                                        {capitalize(status.name)}
                                    </DropdownItem>
                                ))}
                            </DropdownMenu>
                        </Dropdown>
                    </div>
                </div>
                <div className="flex justify-between items-center">
                    <span className="text-default-400 text-small">Total {users.length} users</span>
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
        );
    }, [
        filterValue,
        statusFilter,
        onSearchChange,
        onRowsPerPageChange,onClear
    ]);

    const bottomContent = React.useMemo(() => {
        return (
            <div className="py-2 px-2 flex justify-between items-center">
        <span className="w-[30%] text-small text-default-400">
          {selectedKeys === "all"
              ? "All items selected"
              : `${selectedKeys.size} of ${filteredItems.length} selected`}
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
                    <Button isDisabled={pages === 1} size="sm" variant="flat" onPress={onPreviousPage}>
                        Previous
                    </Button>
                    <Button isDisabled={pages === 1} size="sm" variant="flat" onPress={onNextPage}>
                        Next
                    </Button>
                </div>
            </div>
        );
    }, [selectedKeys, page, pages, filteredItems.length, onNextPage ,onPreviousPage]);

    return (
        <Table
            isHeaderSticky
            aria-label="Example table with custom cells, pagination and sorting"
            bottomContent={bottomContent}
            bottomContentPlacement="outside"
            classNames={{
                wrapper: "max-h-[382px]",
            }}
            selectedKeys={selectedKeys}
            selectionMode="multiple"
            sortDescriptor={sortDescriptor}
            topContent={topContent}
            topContentPlacement="outside"
            onSelectionChange={setSelectedKeys}
            onSortChange={setSortDescriptor}
        >
            <TableHeader columns={headerColumns}>
                {(column) => (
                    <TableColumn
                        key={column.uid}
                        align={column.uid === "actions" ? "center" : "start"}
                        allowsSorting={column.sortable}
                    >
                        {column.name}
                    </TableColumn>
                )}
            </TableHeader>
            <TableBody emptyContent={"No users found"} items={sortedItems}>
                {(item) => (
                    <TableRow key={item.id}>
                        {(columnKey) => <TableCell>{renderCell(item, columnKey)}</TableCell>}
                    </TableRow>
                )}
            </TableBody>
        </Table>
    );
}
