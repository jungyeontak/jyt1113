# KAKAOPAY 사전과제

- 개발 프레임워크
  - .Net framework4.6.1
  - IIS6.0
  
## 문제 해결
- 데이터베이스(테이블) 구조  

|컬럼명|컬럼타입|비고|
|------|---|---|
|COUPON_KEY|VARCHAR2(20)|쿠폰키|
|STATUS|VARCHAR2(1)|상태값(I:쿠폰생성, B:쿠폰번호 발급(구매), C:쿠폰사용취소(사용 후 취소), Y:사용완료, E:만료)|
|CALL_SITE|VARCHAR2(500)|호출한 사이트 주소|
|CREATE_DATE|DATE|쿠폰 생성일자|
|BUY_DATE|DATE|쿠폰 발급일자(구매일자)|
|USE_DATE|DATE|쿠폰 사용일자|
|CANCEL_DATE|DATE|쿠폰 사용취소일자|
|EXPIRE_DATE|DATE|만료된 쿠폰번호 업데이트 일자|
|EXPIRE_SMS_YN|VARCHAR(1)|만료3일전 SMS 전송여부|


- UNIQUE한 쿠폰번호를 어떻게 생각 할 것인지?
  - 쿠폰 길이의 제한이 없었기에 임의로 16자리로 생각하여 만듬 .Net framework Random 함수 사용하여(6자리의 난수를 발생 시키고) 뒤에 10자리는     DataBase(ORACLE)의 SEQUENCE를 사용하여 UNIQUE한 쿠폰코드 생성함. 
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
 - **생각: 데이터베이스에 안전한 방법으로 저장하는 법은 SHA256암호화 기법으로 저장**
 
 - 100억개 이상 쿠폰 관리 저장 관리 가능하도록 구현할것
 - **생각: 오라클에서는 파티션 테이블을 활용하여 월 단위 혹은 년 단위로 데이터를 관리한다.**
 
 - 10만개 이상 벌크 csv Import 기능
 - **생각: .Net framework 에서 제공하는 StreamReader를 이용해서 가져오면 10만개도 충분히 import 가능하다.**
 
  - 대용량 트랙픽(TPS 10K 이상)을 고려한 시스템 구현
   - **생각:**
  - 성능테스트 결과 / 피드백
   - **생각:**
