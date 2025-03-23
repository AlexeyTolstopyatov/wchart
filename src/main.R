# Microsoft Word document analyser
# returns....

# officer package required.


# wc_extract_path
#
# Returns detected Microsoft Word document's
# path from Command-Line arguments
wc_extract_path <- function() {
  args <- commandArgs(trailingOnly = TRUE)

  # arguments filtering
  word_files <- grep("\\.docx?$", args, value = TRUE, ignore.case = TRUE)

  # relative paths check
  is_absolute <- function(path) {
    grepl("^(/|[A-Za-z]:)", path)
  }

  full_word_paths <- word_files[lapply(word_files, is_absolute)]

  # result:
  if (length(full_word_paths) > 0) {
    cat("Microsoft Documents:\n")
    print(full_word_paths)
  } else if (length(word_files) > 0) {
    cat("Relative paths:\n")
    print(word_files)
  } else {
    cat("No selected Microsoft Documents")
  }
  # first time:
  # return first element of sequence
  return(full_word_paths)
}
