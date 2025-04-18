import { NoticeBarProps } from './NoticeBar';
export declare const NoticeBar: import("../utils").WithInstall<import("vue").DefineComponent<{
    text: StringConstructor;
    mode: import("vue").PropType<import("./types").NoticeBarMode>;
    color: StringConstructor;
    delay: {
        type: (NumberConstructor | StringConstructor)[];
        default: number;
    };
    speed: {
        type: (NumberConstructor | StringConstructor)[];
        default: number;
    };
    leftIcon: StringConstructor;
    wrapable: BooleanConstructor;
    background: StringConstructor;
    scrollable: {
        type: import("vue").PropType<boolean | null>;
        default: null;
    };
}, () => import("vue/jsx-runtime").JSX.Element, unknown, {}, {}, import("vue").ComponentOptionsMixin, import("vue").ComponentOptionsMixin, ("close" | "replay")[], "close" | "replay", import("vue").PublicProps, Readonly<import("vue").ExtractPropTypes<{
    text: StringConstructor;
    mode: import("vue").PropType<import("./types").NoticeBarMode>;
    color: StringConstructor;
    delay: {
        type: (NumberConstructor | StringConstructor)[];
        default: number;
    };
    speed: {
        type: (NumberConstructor | StringConstructor)[];
        default: number;
    };
    leftIcon: StringConstructor;
    wrapable: BooleanConstructor;
    background: StringConstructor;
    scrollable: {
        type: import("vue").PropType<boolean | null>;
        default: null;
    };
}>> & {
    onClose?: ((...args: any[]) => any) | undefined;
    onReplay?: ((...args: any[]) => any) | undefined;
}, {
    scrollable: boolean | null;
    delay: string | number;
    speed: string | number;
    wrapable: boolean;
}, {}>>;
export default NoticeBar;
export { noticeBarProps } from './NoticeBar';
export type { NoticeBarProps };
export type { NoticeBarMode, NoticeBarInstance, NoticeBarThemeVars, } from './types';
declare module 'vue' {
    interface GlobalComponents {
        VanNoticeBar: typeof NoticeBar;
    }
}
