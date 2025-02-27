import React from 'react';


export default function Component({ params }: { params: Promise<{ submissionid: string }> }) {
    const paramsId=React.use(params);
    console.log("submit ",paramsId.submissionid)
    return (
        <div>Candidate Id: {paramsId.submissionid}</div>
    );
}
