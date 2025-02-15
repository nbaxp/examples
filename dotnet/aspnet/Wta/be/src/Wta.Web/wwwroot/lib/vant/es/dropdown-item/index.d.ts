import { DropdownItemProps } from './DropdownItem';
export declare const DropdownItem: import("../utils").WithInstall<import("vue").DefineComponent<{
    title: StringConstructor;
    options: {
        type: import("vue").PropType<import("./types").DropdownItemOption[]>;
        default: () => never[];
    };
    disabled: BooleanConstructor;
    teleport: import("vue").PropType<string | import("vue").RendererElement | null | undefined>;
    lazyRender: {
        type: BooleanConstructor;
        default: true;
    };
    modelValue: import("vue").PropType<unknown>;
    titleClass: import("vue").PropType<unknown>;
}, (() => import("vue/jsx-runtime").JSX.Element) | undefined, unknown, {}, {}, import("vue").ComponentOptionsMixin, import("vue").ComponentOptionsMixin, ("open" | "update:modelValue" | "change" | "close" | "opened" | "closed")[], "open" | "update:modelValue" | "change" | "close" | "opened" | "closed", import("vue").PublicProps, Readonly<import("vue").ExtractPropTypes<{
    title: StringConstructor;
    options: {
        type: import("vue").PropType<import("./types").DropdownItemOption[]>;
        default: () => never[];
    };
    disabled: BooleanConstructor;
    teleport: import("vue").PropType<string | import("vue").RendererElement | null | undefined>;
    lazyRender: {
        type: BooleanConstructor;
        default: true;
    };
    modelValue: import("vue").PropType<unknown>;
    titleClass: import("vue").PropType<unknown>;
}>> & {
    onChange?: ((...args: any[]) => any) | undefined;
    "onUpdate:modelValue"?: ((...args: any[]) => any) | undefined;
    onOpen?: ((...args: any[]) => any) | undefined;
    onClose?: ((...args: any[]) => any) | undefined;
    onOpened?: ((...args: any[]) => any) | undefined;
    onClosed?: ((...args: any[]) => any) | undefined;
}, {
    disabled: boolean;
    lazyRender: boolean;
    options: import("./types").DropdownItemOption[];
}, {}>>;
export default DropdownItem;
export { dropdownItemProps } from './DropdownItem';
export type { DropdownItemProps };
export type { DropdownItemOption, DropdownItemInstance, DropdownItemThemeVars, } from './types';
declare module 'vue' {
    interface GlobalComponents {
        VanDropdownItem: typeof DropdownItem;
    }
}
