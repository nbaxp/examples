import { type ExtractPropTypes } from 'vue';
export declare const ACTION_BAR_KEY: unique symbol;
export declare const actionBarProps: {
    placeholder: BooleanConstructor;
    safeAreaInsetBottom: {
        type: BooleanConstructor;
        default: true;
    };
};
export type ActionBarProps = ExtractPropTypes<typeof actionBarProps>;
declare const _default: import("vue").DefineComponent<{
    placeholder: BooleanConstructor;
    safeAreaInsetBottom: {
        type: BooleanConstructor;
        default: true;
    };
}, () => import("vue/jsx-runtime").JSX.Element, unknown, {}, {}, import("vue").ComponentOptionsMixin, import("vue").ComponentOptionsMixin, {}, string, import("vue").PublicProps, Readonly<ExtractPropTypes<{
    placeholder: BooleanConstructor;
    safeAreaInsetBottom: {
        type: BooleanConstructor;
        default: true;
    };
}>>, {
    placeholder: boolean;
    safeAreaInsetBottom: boolean;
}, {}>;
export default _default;
