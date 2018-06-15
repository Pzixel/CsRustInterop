#[repr(C)]
pub struct ArrayRef<T> {
    bytes: *const T,
    length: usize
}

#[no_mangle]
pub extern fn println(array: *const ArrayRef<u8>, count: usize) -> i32 {
    let array_slice = unsafe { std::slice::from_raw_parts(array, count) };
    for str in array_slice {
        let slice = unsafe { std::slice::from_raw_parts(str.bytes, str.length) };
        let str = std::str::from_utf8(slice);
        match str {
            Ok(str) => {
                println!("{}", str);
            },
            Err(_) => return -1
        };
    }
    1
}

#[no_mangle]
pub extern fn get_range(count: usize, callback: extern fn(ArrayRef<i32>)) {
    let range: Vec<_> = (1..count).map(|x| x as i32).collect();
    let array_ref = ArrayRef {
        bytes: range.as_ptr(),
        length: range.len()
    };
    callback(array_ref)
}
