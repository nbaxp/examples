import { type PropType } from 'vue';
import { type Numeric, type Interceptor } from '../utils';
import type { UploaderFileListItem } from './types';
declare const _default: import("vue").DefineComponent<{
    name: (NumberConstructor | StringConstructor)[];
    item: {
        type: PropType<UploaderFileListItem>;
        required: true;
    };
    index: NumberConstructor;
    imageFit: PropType<import("csstype").Property.ObjectFit | undefined>;
    lazyLoad: BooleanConstructor;
    deletable: BooleanConstructor;
    reupload: BooleanConstructor;
    previewSize: PropType<Numeric | [Numeric, Numeric]>;
    beforeDelete: PropType<Interceptor>;
}, () => import("vue/jsx-runtime").JSX.Element, unknown, {}, {}, import("vue").ComponentOptionsMixin, import("vue").ComponentOptionsMixin, ("delete" | "reupload" | "preview")[], "delete" | "reupload" | "preview", import("vue").PublicProps, Readonly<import("vue").ExtractPropTypes<{
    name: (NumberConstructor | StringConstructor)[];
    item: {
        type: PropType<UploaderFileListItem>;
        required: true;
    };
    index: NumberConstructor;
    imageFit: PropType<import("csstype").Property.ObjectFit | undefined>;
    lazyLoad: BooleanConstructor;
    deletable: BooleanConstructor;
    reupload: BooleanConstructor;
    previewSize: PropType<Numeric | [Numeric, Numeric]>;
    beforeDelete: PropType<Interceptor>;
}>> & {
    onDelete?: ((...args: any[]) => any) | undefined;
    onReupload?: ((...args: any[]) => any) | undefined;
    onPreview?: ((...args: any[]) => any) | undefined;
}, {
    lazyLoad: boolean;
    deletable: boolean;
    reupload: boolean;
}, {}>;
export default _default;
