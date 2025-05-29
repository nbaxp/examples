<template>
    <view class="flex flex-direction-column page">
        <view class="flex-shrink-0 flex">
            <view class="padding-15">
                <navigator url="/pages/address/city"><text>{{city}}</text><uni-icons class=".padding-l-15 "
                        type="down" /></navigator>
            </view>
            <uni-search-bar class="flex-1" radius="5" cancelButton="none" v-model="search" />
        </view>
        <view class="relative flex-1 flex w-full">
            <view class="flex-shrink-0">
                <map id="map" v-if="latitude&&longitude" show-location :latitude="latitude" :longitude="longitude"
                    :markers=markers @regionchange="onRegionChange" />
            </view>
            <view class="absolute w-full overflow-y-auto"
                style="box-sizing: border-box;margin-top: 50vw;height:calc(100% - 50vw)">
                <uni-list class="list w-full">
                    <uni-list-item v-for="item in list" :title="item.title" :note="item.address"
                        :right-text="formatDistance(item._distance)" @tap="selectAddress(item.name)" />
                </uni-list>
            </view>
            <view class="absolute w-full h-full overflow-y-auto" v-if="search">
                <uni-list class="list">
                    <uni-list-item v-for="item in searchList" clickable :title="item.title" :note="item.address"
                        :right-text="formatDistance(item._distance)" @tap="selectAddress(item.name)" />
                </uni-list>
            </view>
        </view>
    </view>
</template>

<script setup>
    import {
        ref,
        watchEffect,
        onMounted
    } from 'vue';
    import md5 from '/libs/blueimp-md5/index.js';
    import {
        debounce,
        formatDistance
    } from '/utils/index.js';
    import env from '/env.js'

    const search = ref('');
    const searchList = ref([]);
    const address = ref('');
    const latitude = ref(null);
    const longitude = ref(null);
    const markers = ref([]);
    const list = ref([]);
    const city = ref(null);

    const onRegionChange = async () => {
        const ctx = uni.createMapContext('map');
        const result = await ctx.getCenterLocation({
            type: "gcj02"
        });
        latitude.value = result.latitude;
        longitude.value = result.longitude;
    };

    const mapRequest = async (params, path = '/ws/geocoder/v1/') => {
        const key = env.lbs_key;
        const secretKey = env.lbs_secretKey;
        params = {
            ...params,
            key
        };
        const queryStr = Object.keys(params).sort().map(k => `${k}=${params[k]}`).join('&');
        const sigSource = `${path}?${queryStr}${secretKey}`;
        const sig = md5(sigSource);
        const url = `https://apis.map.qq.com${path}?${queryStr}&sig=${sig}`;
        const [err, res] = await uni.request({
            url
        }).then(res => [null, res]).catch(err => [err, null]);
        if (err) {
            console.error('请求失败', err);
            throw new Error('地图请求失败');
        }
        if (res.data.status !== 0) {
            console.error('腾讯地图返回错误', res.data);
            throw new Error('腾讯地图接口错误：' + res.data.message);
        }
        console.log(res);
        return res.data;
    };

    const getAddressByRegionChange = debounce(async () => {
        const params = {
            location: `${latitude.value},${longitude.value}`,
            get_poi: 1
        };
        const data = await mapRequest(params);
        city.value = data.result.address_component.city;
        list.value = data.result.pois;
        markers.value = [{
            id: 0,
            latitude: latitude.value,
            longitude: longitude.value,
            iconPath: '/static/images/location.png',
            width: 25,
            height: 25
        }];
    }, 1000);

    const getAddressBySearchChange = debounce(async () => {
        const params = {
            keyword: search.value,
            boundary: `region(${city.value},0,${latitude.value},${longitude.value})`,
            page_size: 20,
            page_index: 1
        };
        const data = await mapRequest(params, '/ws/place/v1/search/');
        searchList.value = data.data;
    }, 1000);

    watchEffect(() => {
        if (latitude.value && longitude.value) {
            getAddressByRegionChange();
        }
    });

    watchEffect(() => {
        if (latitude.value && longitude.value && city.value && search.value) {
            getAddressBySearchChange();
        }
    });

    onMounted(async () => {
        uni.$on('city', o => {
            city.value = o;
        });
        const result = await uni.getLocation({
            type: "gcj02"
        });
        latitude.value = result.latitude;
        longitude.value = result.longitude;
    });
</script>

<style>
    map {
        width: 100vw;
        height: 50vw;
    }
</style>