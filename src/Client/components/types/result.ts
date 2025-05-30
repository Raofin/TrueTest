export interface Candidate {
    account: {
        accountId: string;
        username: string;
        email: string;
    };
    result: {
        totalScore: number;
        problemSolvingScore: number;
        writtenScore: number;
        mcqScore: number;
        startedAt: string;
        submittedAt: string;
        hasCheated: boolean,
        isReviewed: boolean
    };
}
