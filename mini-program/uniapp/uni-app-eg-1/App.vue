<script>
    export default {
        onLaunch: function() {
            console.log(uni.getSystemInfoSync().platform);
            console.log('App Launch')
        },
        onShow: function() {
            console.log('App Show')
        },
        onHide: function() {
            console.log('App Hide')
        }
    }
</script>

<style>
    :not(not) {
        box-sizing: border-box;
    }

    .h-full {
        height: 100%;
    }

    .w-full {
        width: 100%;
    }

    .page-with-bar {
        height: calc(100vh - var(--window-top) - var(--window-bottom) - 44px);
    }

    .page {
        height: calc(100vh - var(--window-top) - var(--window-bottom));
    }

    .absolute {
        position: absolute;
    }

    .relative {
        position: relative;
    }

    .inline {
        display: inline;
    }

    .inline-block {
        display: inline-block;
    }

    .top-0 {
        top: 0;
    }

    .right-0 {
        right: 0;
    }

    .position-fixed {
        position: fixed;
    }

    .flex {
        display: flex;
    }

    .inline-flex {
        display: inline-flex;
    }

    .flex-direction-column {
        flex-direction: column;
    }

    .align-items-center {
        align-items: center;
    }

    .align-items-end {
        align-items: end;
    }

    .justify-content-center {
        justify-content: center;
    }

    .justify-content-between {
        justify-content: space-between;
    }

    .text-align-center {
        text-align: center;
    }

    .vertical-align-middle {
        vertical-align: middle;
    }

    .padding-15 {
        padding: 15px;
    }


    .padding-x-15 {
        padding-left: 15px;
        padding-right: 15px;
    }

    .padding-y-15 {
        padding-top: 15px;
        padding-bottom: 15px;
    }

    .padding-l-15 {
        padding-left: 15px;
    }

    .margin-0 {
        margin: 0;
    }

    .margin-right-0 {
        margin-right: 0;
    }

    .flex-1 {
        flex: 1;
    }

    .flex-shrink-0 {
        flex-shrink: 0;
    }

    .width-auto {
        width: auto;
    }

    .overflow-y-auto {
        overflow-y: auto;
    }

    .overflow-visible {
        overflow: auto;
    }

    .form-item {
        display: flex;
        padding: 15px;
    }
</style>