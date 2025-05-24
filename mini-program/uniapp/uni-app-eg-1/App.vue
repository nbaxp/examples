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
        width: 100%;
    }

    .w-full {
        width: 100%;
    }

    .position-absolute {
        position: absolute;
    }

    .position-fixed {
        position: fixed;
    }

    .flex {
        display: flex;
    }

    .flex-direction-column {
        flex-direction: column;
    }

    .align-items-center {
        align-items: center;
    }

    .justify-content-center {
        justify-content: center;
    }

    .justify-content-between {
        justify-content: space-between;
    }

    .padding-15 {
        padding: 15px;
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
</style>