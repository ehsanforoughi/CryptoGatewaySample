
export class MyGrid {
    prop: string | undefined;
    name: string | undefined;
    isDecimal?: boolean = false;
    numberPipe?: string = "";
    direction?: string = "rtl";
    isIcon?: boolean = false;
    iconSize?: number = 32;
    isHyperLink?: boolean = false;
    hyperLinkText?: string = "";
    additionalLabel?: AdditionalLabel = new AdditionalLabel();
    additionalLineText?: AdditionalLineText = new AdditionalLineText();
    removeParentheses?: boolean = false;
    isLong?: boolean = false;
}

export class AdditionalLabel {
    has?: boolean = false;
    text?: string = "";
    condition?: (row: any) => boolean;  
}

export class AdditionalLineText {
    has?: boolean = false;
    columnName?: string = "";
    condition?: (row: any) => boolean;  
}