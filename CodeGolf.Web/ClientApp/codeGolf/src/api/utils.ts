import { AxiosResponse } from "axios";

import { ChallengeSet, Hole } from "../types/types";

export const MapHole = (h?: HoleInt) => {
    if (h) {
        return { ...h, start: utzParse(h.start), end: utzParse(h.end), closedAt: h.closedAt ? utzParse(h.closedAt) : undefined } as Hole;
    }

    // tslint:disable-next-line: no-return-undefined
    return undefined;
};

export const utzParse = (date: string) => new Date(date + "z");

export interface HoleInt { readonly challengeSet: ChallengeSet; readonly start: string; readonly end: string; readonly closedAt?: string; }

export const getData = <T>(r: AxiosResponse<T>) => r.data;

export const JsonHeaders = {
    "Content-Type": "application/json"
};
