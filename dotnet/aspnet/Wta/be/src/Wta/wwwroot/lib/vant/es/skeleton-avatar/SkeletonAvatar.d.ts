import { ExtractPropTypes } from 'vue';
export type SkeletonAvatarShape = 'square' | 'round';
export declare const skeletonAvatarProps: {
    avatarSize: (NumberConstructor | StringConstructor)[];
    avatarShape: {
        type: import("vue").PropType<SkeletonAvatarShape>;
        default: SkeletonAvatarShape;
    };
};
export type SkeletonAvatarProps = ExtractPropTypes<typeof skeletonAvatarProps>;
declare const _default: import("vue").DefineComponent<{
    avatarSize: (NumberConstructor | StringConstructor)[];
    avatarShape: {
        type: import("vue").PropType<SkeletonAvatarShape>;
        default: SkeletonAvatarShape;
    };
}, () => import("vue/jsx-runtime").JSX.Element, unknown, {}, {}, import("vue").ComponentOptionsMixin, import("vue").ComponentOptionsMixin, {}, string, import("vue").PublicProps, Readonly<ExtractPropTypes<{
    avatarSize: (NumberConstructor | StringConstructor)[];
    avatarShape: {
        type: import("vue").PropType<SkeletonAvatarShape>;
        default: SkeletonAvatarShape;
    };
}>>, {
    avatarShape: SkeletonAvatarShape;
}, {}>;
export default _default;
