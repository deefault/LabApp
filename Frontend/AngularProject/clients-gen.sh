#!/usr/bin/env bash

BASE_PATH='/local/src/app/clients'
URL = 'http://localhost:5002'
gen_student () {
  docker run --network=host --rm -v ${PWD}:/local swaggerapi/swagger-codegen-cli-v3 generate \
       -i $URL/swagger/student/swagger.json \
        -l typescript-angular \
        -o "${BASE_PATH}/student" 
}

gen_teacher () {
  docker run --network=host --rm -v ${PWD}:/local swaggerapi/swagger-codegen-cli-v3 generate \
          -i $URL/swagger/teacher/swagger.json \
           -l typescript-angular \
           -o "${BASE_PATH}/teacher" 
}

gen_common () {
  docker run --network=host --rm -v ${PWD}:/local swaggerapi/swagger-codegen-cli-v3 generate \
         -i $URL/swagger/common/swagger.json \
          -l typescript-angular \
          -o "${BASE_PATH}/common" 
}

gen_all() {
  echo 'generating student client...'
  gen_student
  echo 'generating teacher client...'
  gen_teacher
  echo 'generating common client...'
  gen_common
  echo 'FINISHED'
}


if [[ $# -eq 0 ]]; then
    gen_all
elif [[ $# -eq 1 ]]; then
         if [[ $1 == 'student' ]]; then
            gen_student
         elif [[ $1 == 'teacher' ]]; then
            gen_teacher
         elif [[ $1 == 'common' ]]; then
            gen_common
         else
            echo 'Wrong argument'
         fi
else
  echo 'Wrong argument'
fi
