<template>
    <view class="flex flex-direction-column page">
        <view class="flex-shrink-0">
            <uni-search-bar class="w-full" radius="5" cancelButton="none" v-model="search" />
        </view>
        <view id='tabs' class="relative flex-1 flex w-full overflow-y-auto">
            <scroll-view v-if="!search" class="list-container width-auto flex-1" scroll-y :scroll-into-view="listId"
                @scroll="onListScroll">
                <uni-list v-for="item in letters" class="list" :id="'id_'+item">
                    <uni-list-item showExtraIcon :extraIcon="{type:'list'}" :title="item" />
                    <uni-list-item :title="item.name" clickable v-for="item in list.filter(o=>o.letter===item)"
                        @tap="selectCity(item.name)" />
                </uni-list>
            </scroll-view>
            <view v-if="!search" class="menu flex flex-direction-column justify-content-center">
                <view v-for="item in letters" :key="item" :class="['menu-item', item === activeIndex ? 'active' : '']"
                    @tap="selectCategory(item)">
                    {{ item }}
                </view>
            </view>
            <view class="search absolute w-full h-full overflow-y-auto" v-if="search">
                <uni-list class="list">
                    <uni-list-item :title="item.name" clickable
                        v-for="item in list.filter(o=>o.name.indexOf(search)>-1)" @tap="selectCity(item.name)" />
                </uni-list>
            </view>
        </view>
    </view>
</template>

<script setup>
    import {
        ref,
        watchEffect,
        nextTick
    } from 'vue';
    import {
        delay
    } from '/utils/index.js';

    const search = ref('');
    const list = ref([]);
    const listId = ref('id_' + 'A');
    const scrollTop = ref(0);
    const activeIndex = ref('A');
    const itemTops = ref([]);
    const menuScrollTop = ref(0);
    const letters = ref([...Array(26)].map((_, i) => String.fromCharCode(65 + i)));

    const selectCategory = (e) => {
        listId.value = 'id_' + e;
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
                        activeIndex.value = String.fromCharCode(65 + i);
                        const currentMenuId = `id_${String.fromCharCode(65+i)}`;
                        uni.createSelectorQuery().select('#tabs').boundingClientRect(tabs => {
                            const tabsHeight = tabs.height;
                            uni.createSelectorQuery()
                                .select(`#${currentMenuId}`)
                                .boundingClientRect(menu => {
                                    const menuTop = menu.top - offsetTop;
                                    if (menuTop >= 0 && menuTop <= tabsHeight - menu.height) {
                                        //当前项在可视区域中
                                    } else if (menuTop < 0) {
                                        //在可视区域上面
                                        const newValue = menuScrollTop.value + menuTop;
                                        if (menuScrollTop.value !== newValue) {
                                            menuScrollTop.value = newValue;
                                        }
                                    } else {
                                        //在可视区域下面
                                        const newValue = menuScrollTop.value + (menuTop + menu
                                            .height - tabsHeight);
                                        if (menuScrollTop.value !== newValue) {
                                            menuScrollTop.value = newValue;
                                        }
                                    }
                                })
                                .exec();
                        }).exec();
                        break;
                    }
                }
            })
            .exec();
    };

    const selectCity = (e) => {
        uni.navigateBack({
            success() {
                uni.$emit('city', e)
            }
        });
        console.log(e);
    };

    //测试数据
    // for (let i = 0; i < 26; i++) {
    //     for (let j = 0; j < 10; j++) {
    //         list.value.push({
    //             number: i * 100 + j,
    //             name: (i * 100 + j) + '分类',
    //             letter: String.fromCharCode(65 + i)
    //         });
    //     }
    // }
    list.value = [{
            "letter": "A",
            "number": 1,
            "name": "鞍山市"
        },
        {
            "letter": "B",
            "number": 2,
            "name": "北京市"
        },
        {
            "letter": "C",
            "number": 3,
            "name": "重庆市"
        },
        {
            "letter": "D",
            "number": 4,
            "name": "大连市"
        },
        {
            "letter": "F",
            "number": 5,
            "name": "福州市"
        },
        {
            "letter": "G",
            "number": 6,
            "name": "广州市"
        },
        {
            "letter": "H",
            "number": 7,
            "name": "杭州市"
        },
        {
            "letter": "J",
            "number": 8,
            "name": "济南市"
        },
        {
            "letter": "K",
            "number": 9,
            "name": "昆明市"
        },
        {
            "letter": "L",
            "number": 10,
            "name": "兰州市"
        },
        {
            "letter": "N",
            "number": 11,
            "name": "南京市"
        },
        {
            "letter": "Q",
            "number": 12,
            "name": "青岛市"
        },
        {
            "letter": "S",
            "number": 13,
            "name": "上海市"
        },
        {
            "letter": "T",
            "number": 14,
            "name": "天津市"
        },
        {
            "letter": "W",
            "number": 15,
            "name": "武汉市"
        },
        {
            "letter": "X",
            "number": 16,
            "name": "西安市"
        },
        {
            "letter": "Y",
            "number": 17,
            "name": "银川市"
        },
        {
            "letter": "Z",
            "number": 18,
            "name": "郑州市"
        }
    ];
</script>

<style>
    .menu-item {
        padding: 1px 5px;
        text-align: center;
        background-color: #fff;
    }

    .menu-item.active {
        background-color: green;
        color: #fff;
    }

    uni-section {
        background-color: #eee;
    }

    .list-container .list:last-child {
        min-height: calc(100vh - var(--window-top));
    }

    .search {
        background-color: #fff;
    }
</style>