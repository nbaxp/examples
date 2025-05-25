<template>
    <view>
        <uni-nav-bar class="w-full" status-bar fixed :border="false" left-icon="search" @clickLeft="onSearchClick"
            title="分类" />
        <view id='tabs' class="flex w-full page-with-bar">
            <scroll-view class="width-auto" scroll-y :scroll-into-view="menuId" :scroll-top="menuScrollTop"
                @scroll="onMenuScroll">
                <view v-for="(item, index) in list" :key="index" :id="'id_'+index"
                    :class="['menu-item', index === activeIndex ? 'active' : '']" @tap="selectCategory(index)">
                    {{ item.name }}
                </view>
            </scroll-view>
            <scroll-view class="list-container width-auto flex-1" scroll-y :scroll-into-view="listId"
                @scroll="onListScroll">
                <view v-for="(item, index) in list" :key="index" :id="'id_'+index" class="list">
                    <view class="padding-15"><text>title:{{ item.name }}</text></view>
                    <view class="w-full flex-1 flex align-items-center padding-15" v-for="i in 3">
                        <uni-icons type="image" :size="60" />
                        <text class="text">{{item.name}} {{i}}</text>
                    </view>
                </view>
            </scroll-view>
        </view>
    </view>
</template>

<script setup>
    import {
        ref,
        watchEffect,
        nextTick
    } from 'vue';

    const list = ref([]);
    const menuId = ref('id_' + 0);
    const listId = ref('id_' + 0);
    const scrollTop = ref(0);
    const activeIndex = ref(0);
    const itemTops = ref([]);
    const menuScrollTop = ref(0);

    const selectCategory = (index) => {
        listId.value = 'id_' + index;
    };

    const onMenuScroll = e => {
        menuScrollTop.value = e.detail.scrollTop;
    };

    const onListScroll = (e) => {
        const offsetTop = e.target.offsetTop;
        uni.createSelectorQuery()
            .selectAll('.list')
            .boundingClientRect(list => {
                for (let i = 0; i < list.length; i++) {
                    const item = list[i];
                    const top = item.top - offsetTop;
                    if (top <= 0 && top > -item.height) {
                        activeIndex.value = i;
                        const currentMenuId = `id_${i}`;
                        uni.createSelectorQuery().select('#tabs').boundingClientRect(tabs => {
                            const tabsHeight = tabs.height;
                            uni.createSelectorQuery()
                                .select(`#${currentMenuId}`)
                                .boundingClientRect(menu => {
                                    const menuTop = menu.top - offsetTop;
                                    console.log(
                                        `sta:${currentMenuId}:menuTop:${menuTop}, menuScrollTop.value:${menuScrollTop.value}`
                                    );
                                    // 当前项在可视区域中
                                    if (menuTop >= 0 && menuTop <= tabsHeight - menu.height) {
                                        // 无需滚动
                                    } else if (menuTop < 0) {
                                        const newValue = menuScrollTop.value + menuTop;
                                        if (menuScrollTop.value !== newValue) {
                                            menuScrollTop.value = newValue;
                                        }
                                    } else {
                                        const newValue = menuScrollTop.value + (menuTop + menu
                                            .height - tabsHeight); //此处计算有误
                                        if (menuScrollTop.value !== newValue) {
                                            menuScrollTop.value = newValue;
                                        }

                                    }
                                    console.log(
                                        `end:${currentMenuId}:menu.top:${menuTop}, menuScrollTop.value:${menuScrollTop.value}`
                                    );
                                })
                                .exec();
                        }).exec();
                        break;
                    }
                }
            })
            .exec();
    };

    const onSearchClick = () => {
        uni.navigateTo({
            url: '/pages/search/search'
        })
    };

    for (let i = 0; i < 30; i++) {
        list.value.push({
            name: i + '分类',
        });
    }
</script>

<style>
    .uni-navbar {
        background-color: #f8f8f8;
    }

    .menu-item {
        line-height: 3rem;
        padding: 15px;
        background-color: #eee;
        border-bottom: 1px solid #fff;
    }

    .menu-item.active {
        background-color: #fff;
    }

    .list-container .list:last-child {
        min-height: calc(100vh - var(--window-top) - var(--window-bottom) - 44px);
    }
</style>