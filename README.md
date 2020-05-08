# KAKAOPAY 사전과제

- 개발 프레임워크
  - .Net framework4.6.1
  - IIS6.0
  
## 문제 해결
- UNIQUE한 쿠폰번호를 어떻게 생각 할 것인지?
  - 쿠폰 길이의 제한이 없었기에 임의로 16자리로 생각하여 만듬 .Net framework Random 함수 사용하여(12자리의 난수를 발생 시키고) 뒤에 4자리는     DataBase의 Auto Increment를 사용하여 UNIQUE한 쿠폰코드 생성함.
- 데이터베이스를 구축 할 수 없는데 어떻게 할 것인지?
  - 간단한 데이터베이스를 구성하기 위해 XML파일을 이용하여 가상의 데이터베이스로 만들어 사용하였다.
  
## 빌드 및 실행 방법
1. visual studio 2015이상 설치 후 KaKaoPayRestApi.sln파일을 클릭하여 오픈 한다.
2. F5버튼을 빌드 및 로컬 WebApi 서버를 구동시켜 실행한다.

## 제약사항(선택)
 - API 인증을 위해 JWT(Json Web Token)를 이용해서 Token 기반 API 인증 기능을 개발하고 각 API 호출 시에 HTTP Header에 발급받은 토큰을 가지고 호출하세요
   - signup 계정생성 API: ID, PW를 입력 받아 내부 DB에 계정을 저장하고 토큰을 생성하여 출력한다. 
    - 단, 패스워드는 안전한 방법으로 저장한다. 
   - signin 로그인 API: 입력으로 생성된 계정 (ID, PW)으로 로그인 요청하면 토큰을 발급한다.
   **생각: 데이터베이스에 안전한 방법으로 저장하는 법은 SHA256암호화 기법으로 저장**
