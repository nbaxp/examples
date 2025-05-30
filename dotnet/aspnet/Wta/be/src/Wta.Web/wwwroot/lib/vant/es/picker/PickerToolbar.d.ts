export declare const pickerToolbarProps: {
    title: StringConstructor;
    cancelButtonText: StringConstructor;
    confirmButtonText: StringConstructor;
};
export declare const pickerToolbarSlots: string[];
export type PickerToolbarPropKeys = Array<keyof typeof pickerToolbarProps>;
export declare const pickerToolbarPropKeys: PickerToolbarPropKeys;
declare const _default: import("vue").DefineComponent<{
    title: StringConstructor;
    cancelButtonText: StringConstructor;
    confirmButtonText: StringConstructor;
}, () => import("vue/jsx-runtime").JSX.Element, unknown, {}, {}, import("vue").ComponentOptionsMixin, import("vue").ComponentOptionsMixin, ("cancel" | "confirm")[], "cancel" | "confirm", import("vue").PublicProps, Readonly<import("vue").ExtractPropTypes<{
    title: StringConstructor;
    cancelButtonText: StringConstructor;
    confirmButtonText: StringConstructor;
}>> & {
    onCancel?: ((...args: any[]) => any) | undefined;
    onConfirm?: ((...args: any[]) => any) | undefined;
}, {}, {}>;
export default _default;
