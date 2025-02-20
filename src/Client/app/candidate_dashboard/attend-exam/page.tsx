"use client"
import React from "react";
import '../../../styles/globals.css'
import {CheckboxGroup, Checkbox, Button} from "@heroui/react";
export default function Component() {
    const [selected, setSelected] =React.useState<string[]>([]);
    return(
        <div>
            <ul  className="list-disc ml-96 space-y-12 mt-24">
                <li className={"text-xl"}>Exam Instructions</li>
                <li>
                    <div className="flex flex-col gap-3">
                        <CheckboxGroup
                            color="warning"
                            label="Permissions for camera , audio,screen record"
                            value={selected}
                            onValueChange={setSelected}>
                            <Checkbox value="Allow Camera">Allow Camera</Checkbox>
                            <Checkbox value="Allow audio of your device">Allow audio of your device</Checkbox>
                            <Checkbox value="Allow screen record"> Allow screen record</Checkbox>
                        </CheckboxGroup>
                        <p className="text-default-500 text-small">Selected: {selected.join(", ")}</p>
                    </div>
                </li>
                <li>Give a Practice Exam   <Button color="primary" variant="ghost">
                    Practice
                </Button></li>
            </ul>
            <div className="flex justify-center items-center mt-16 w-full"><Button color="primary" >Attend Exam
            </Button></div>
        </div>
    )
}