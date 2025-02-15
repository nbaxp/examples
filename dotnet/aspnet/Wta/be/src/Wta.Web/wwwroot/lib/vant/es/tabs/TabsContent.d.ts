declare const _default: import("vue").DefineComponent<{
    count: {
        type: NumberConstructor;
        required: true;
    };
    inited: BooleanConstructor;
    animated: BooleanConstructor;
    duration: {
        type: (NumberConstructor | StringConstructor)[];
        required: true;
    };
    swipeable: BooleanConstructor;
    lazyRender: BooleanConstructor;
    currentIndex: {
        type: NumberConstructor;
        required: true;
    };
}, () => import("vue/jsx-runtime").JSX.Element, unknown, {}, {}, import("vue").ComponentOptionsMixin, import("vue").ComponentOptionsMixin, "change"[], "change", import("vue").PublicProps, Readonly<import("vue").ExtractPropTypes<{
    count: {
        type: NumberConstructor;
        required: true;
    };
    inited: BooleanConstructor;
    animated: BooleanConstructor;
    duration: {
        type: (NumberConstructor | StringConstructor)[];
        required: true;
    };
    swipeable: BooleanConstructor;
    lazyRender: BooleanConstructor;
    currentIndex: {
        type: NumberConstructor;
        required: true;
    };
}>> & {
    onChange?: ((...args: any[]) => any) | undefined;
}, {
    lazyRender: boolean;
    inited: boolean;
    animated: boolean;
    swipeable: boolean;
}, {}>;
export default _default;
