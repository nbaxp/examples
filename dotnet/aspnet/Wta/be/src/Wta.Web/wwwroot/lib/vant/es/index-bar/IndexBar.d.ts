import { type PropType, type InjectionKey, type ExtractPropTypes } from 'vue';
import { type Numeric } from '../utils';
import { IndexBarProvide } from './types';
declare function genAlphabet(): string[];
export declare const indexBarProps: {
    sticky: {
        type: BooleanConstructor;
        default: true;
    };
    zIndex: (NumberConstructor | StringConstructor)[];
    teleport: PropType<string | import("vue").RendererElement | null | undefined>;
    highlightColor: StringConstructor;
    stickyOffsetTop: {
        type: NumberConstructor;
        default: number;
    };
    indexList: {
        type: PropType<Numeric[]>;
        default: typeof genAlphabet;
    };
};
export type IndexBarProps = ExtractPropTypes<typeof indexBarProps>;
export declare const INDEX_BAR_KEY: InjectionKey<IndexBarProvide>;
declare const _default: import("vue").DefineComponent<{
    sticky: {
        type: BooleanConstructor;
        default: true;
    };
    zIndex: (NumberConstructor | StringConstructor)[];
    teleport: PropType<string | import("vue").RendererElement | null | undefined>;
    highlightColor: StringConstructor;
    stickyOffsetTop: {
        type: NumberConstructor;
        default: number;
    };
    indexList: {
        type: PropType<Numeric[]>;
        default: typeof genAlphabet;
    };
}, () => import("vue/jsx-runtime").JSX.Element, unknown, {}, {}, import("vue").ComponentOptionsMixin, import("vue").ComponentOptionsMixin, ("select" | "change")[], "select" | "change", import("vue").PublicProps, Readonly<ExtractPropTypes<{
    sticky: {
        type: BooleanConstructor;
        default: true;
    };
    zIndex: (NumberConstructor | StringConstructor)[];
    teleport: PropType<string | import("vue").RendererElement | null | undefined>;
    highlightColor: StringConstructor;
    stickyOffsetTop: {
        type: NumberConstructor;
        default: number;
    };
    indexList: {
        type: PropType<Numeric[]>;
        default: typeof genAlphabet;
    };
}>> & {
    onChange?: ((...args: any[]) => any) | undefined;
    onSelect?: ((...args: any[]) => any) | undefined;
}, {
    sticky: boolean;
    stickyOffsetTop: number;
    indexList: Numeric[];
}, {}>;
export default _default;
