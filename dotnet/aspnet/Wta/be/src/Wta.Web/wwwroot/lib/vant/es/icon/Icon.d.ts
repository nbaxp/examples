import { type PropType, type ExtractPropTypes } from 'vue';
import { type BadgeProps } from '../badge';
export declare const iconProps: {
    dot: BooleanConstructor;
    tag: {
        type: PropType<keyof HTMLElementTagNameMap>;
        default: keyof HTMLElementTagNameMap;
    };
    name: StringConstructor;
    size: (NumberConstructor | StringConstructor)[];
    badge: (NumberConstructor | StringConstructor)[];
    color: StringConstructor;
    badgeProps: PropType<Partial<BadgeProps>>;
    classPrefix: StringConstructor;
};
export type IconProps = ExtractPropTypes<typeof iconProps>;
declare const _default: import("vue").DefineComponent<{
    dot: BooleanConstructor;
    tag: {
        type: PropType<keyof HTMLElementTagNameMap>;
        default: keyof HTMLElementTagNameMap;
    };
    name: StringConstructor;
    size: (NumberConstructor | StringConstructor)[];
    badge: (NumberConstructor | StringConstructor)[];
    color: StringConstructor;
    badgeProps: PropType<Partial<BadgeProps>>;
    classPrefix: StringConstructor;
}, () => import("vue/jsx-runtime").JSX.Element, unknown, {}, {}, import("vue").ComponentOptionsMixin, import("vue").ComponentOptionsMixin, {}, string, import("vue").PublicProps, Readonly<ExtractPropTypes<{
    dot: BooleanConstructor;
    tag: {
        type: PropType<keyof HTMLElementTagNameMap>;
        default: keyof HTMLElementTagNameMap;
    };
    name: StringConstructor;
    size: (NumberConstructor | StringConstructor)[];
    badge: (NumberConstructor | StringConstructor)[];
    color: StringConstructor;
    badgeProps: PropType<Partial<BadgeProps>>;
    classPrefix: StringConstructor;
}>>, {
    dot: boolean;
    tag: keyof HTMLElementTagNameMap;
}, {}>;
export default _default;
