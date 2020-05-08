# KAKAOPAY 사전과제

- 개발 프레임워크
  - .Net framework4.6.1
  - IIS6.0
  
## 문제 해결
- UNIQUE한 쿠폰번호를 어떻게 생각 할 것인지?
  - 쿠폰 길이의 제한이 없었기에 임의로 16자리로 생각하여 만듬 .Net framework Random 함수 사용하여(12자리의 난수를 발생 시키고) 뒤에 4자리는     DataBase의 Auto Increment를 사용하여 UNIQUE한 쿠폰코드 생성함.

## 빌드 및 실행 방법
1. visual studio 2015이상 설치 후 KaKaoPayRestApi.sln파일을 클릭하여 오픈 한다.
2. F5버튼을 빌드 및 로컬 WebApi 서버를 구동시켜 실행한다.
