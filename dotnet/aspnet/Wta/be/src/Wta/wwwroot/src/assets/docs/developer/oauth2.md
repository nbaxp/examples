# 应用集成

支持 OAuth2 授权码模式集成外部应用，参考 <https://gitee.com/api/v5/oauth_doc#/>

## 获取 Authorization Code

应用在浏览器中携带 client_id 跳转到 OAuth2 Server 网站，OAuth Server 接受请求，生成 code, 携带 code 跳转回到应用

1. 请求地址: [OAuth2 服务端]/api/oauth/authorize
1. 请求参数: client_id={client_id}&redirect_uri={redirect_uri}&response_type=code
1. HTTP Method: GET
1. 回调地址: [OAuth2 客户端]{redirect_uri}?code=abc&state=xyz

## 通过Authorization Code获取Access Token

应用接受 OAuth2 Server 的回调，在应用服务端通过 code 请求 OAuth2 Server 获取 access_token

## OAuth2 Server 处理

浏览器跳转到 OAuth Server 时，如果已经登录，则生成 code 直接跳转回应用，否则跳转到登录页，登录后再生成 code 跳转回到应用

## OAuth2 Client 处理

浏览器跳转到 OAuth2 Client 时，如果是已登录用户且未绑定该 OAuth2 Server 的 OpenId，则进行绑定，已绑定则无需进行任何处理

浏览器跳转到 OAuth2 Client 时，如果是未登录用户但存在绑定的用户，则自动进行登录，否则进行跳转，可以选择登录并绑定或注册并绑定
