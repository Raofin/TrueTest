export type User = {
    username: string;
    email: string;
    roles: string[];
    createdAt: string;
    isActive: boolean;
    accountId: string;
};
export type ApiResponse = {
    page: {
        index: number;
        size: number;
        totalCount: number;
        totalPages: number;
        hasNext: boolean;
        hasPrevious: boolean;
    };
    accounts: User[];
};