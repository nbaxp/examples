export declare const Row: import("../utils").WithInstall<import("vue").DefineComponent<{
    tag: {
        type: import("vue").PropType<keyof HTMLElementTagNameMap>;
        default: keyof HTMLElementTagNameMap;
    };
    wrap: {
        type: BooleanConstructor;
        default: true;
    };
    align: import("vue").PropType<import("./Row").RowAlign>;
    gutter: {
        type: import("vue").PropType<string | number | (string | number)[]>;
        default: number;
    };
    justify: import("vue").PropType<import("./Row").RowJustify>;
}, () => import("vue/jsx-runtime").JSX.Element, unknown, {}, {}, import("vue").ComponentOptionsMixin, import("vue").ComponentOptionsMixin, {}, string, import("vue").PublicProps, Readonly<import("vue").ExtractPropTypes<{
    tag: {
        type: import("vue").PropType<keyof HTMLElementTagNameMap>;
        default: keyof HTMLElementTagNameMap;
    };
    wrap: {
        type: BooleanConstructor;
        default: true;
    };
    align: import("vue").PropType<import("./Row").RowAlign>;
    gutter: {
        type: import("vue").PropType<string | number | (string | number)[]>;
        default: number;
    };
    justify: import("vue").PropType<import("./Row").RowJustify>;
}>>, {
    tag: keyof HTMLElementTagNameMap;
    wrap: boolean;
    gutter: string | number | (string | number)[];
}, {}>>;
export default Row;
export { rowProps } from './Row';
export type { RowProps, RowAlign, RowJustify } from './Row';
declare module 'vue' {
    interface GlobalComponents {
        VanRow: typeof Row;
    }
}
