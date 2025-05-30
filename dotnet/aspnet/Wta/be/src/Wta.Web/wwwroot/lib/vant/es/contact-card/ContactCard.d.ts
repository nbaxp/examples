import { type ExtractPropTypes } from 'vue';
export type ContactCardType = 'add' | 'edit';
export declare const contactCardProps: {
    tel: StringConstructor;
    name: StringConstructor;
    type: {
        type: import("vue").PropType<ContactCardType>;
        default: ContactCardType;
    };
    addText: StringConstructor;
    editable: {
        type: BooleanConstructor;
        default: true;
    };
};
export type ContactCardProps = ExtractPropTypes<typeof contactCardProps>;
declare const _default: import("vue").DefineComponent<{
    tel: StringConstructor;
    name: StringConstructor;
    type: {
        type: import("vue").PropType<ContactCardType>;
        default: ContactCardType;
    };
    addText: StringConstructor;
    editable: {
        type: BooleanConstructor;
        default: true;
    };
}, () => import("vue/jsx-runtime").JSX.Element, unknown, {}, {}, import("vue").ComponentOptionsMixin, import("vue").ComponentOptionsMixin, "click"[], "click", import("vue").PublicProps, Readonly<ExtractPropTypes<{
    tel: StringConstructor;
    name: StringConstructor;
    type: {
        type: import("vue").PropType<ContactCardType>;
        default: ContactCardType;
    };
    addText: StringConstructor;
    editable: {
        type: BooleanConstructor;
        default: true;
    };
}>> & {
    onClick?: ((...args: any[]) => any) | undefined;
}, {
    type: ContactCardType;
    editable: boolean;
}, {}>;
export default _default;
