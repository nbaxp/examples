export declare const CountDown: import("../utils").WithInstall<import("vue").DefineComponent<{
    time: {
        type: (NumberConstructor | StringConstructor)[];
        default: number;
    };
    format: {
        type: import("vue").PropType<string>;
        default: string;
    };
    autoStart: {
        type: BooleanConstructor;
        default: true;
    };
    millisecond: BooleanConstructor;
}, () => import("vue/jsx-runtime").JSX.Element, unknown, {}, {}, import("vue").ComponentOptionsMixin, import("vue").ComponentOptionsMixin, ("change" | "finish")[], "change" | "finish", import("vue").PublicProps, Readonly<import("vue").ExtractPropTypes<{
    time: {
        type: (NumberConstructor | StringConstructor)[];
        default: number;
    };
    format: {
        type: import("vue").PropType<string>;
        default: string;
    };
    autoStart: {
        type: BooleanConstructor;
        default: true;
    };
    millisecond: BooleanConstructor;
}>> & {
    onChange?: ((...args: any[]) => any) | undefined;
    onFinish?: ((...args: any[]) => any) | undefined;
}, {
    time: string | number;
    format: string;
    autoStart: boolean;
    millisecond: boolean;
}, {}>>;
export default CountDown;
export { countDownProps } from './CountDown';
export type { CountDownProps } from './CountDown';
export type { CountDownInstance, CountDownThemeVars, CountDownCurrentTime, } from './types';
declare module 'vue' {
    interface GlobalComponents {
        VanCountDown: typeof CountDown;
    }
}
