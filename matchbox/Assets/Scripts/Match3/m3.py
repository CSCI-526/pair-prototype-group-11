import random
import time 

class Match3Game:
    BOARD_SIZE = 5

    def __init__(self):
        self.board = [[0 for _ in range(self.BOARD_SIZE)] for _ in range(self.BOARD_SIZE)]
        self.initialize_board()

    def initialize_board(self):

        for row in range(self.BOARD_SIZE):
            for col in range(self.BOARD_SIZE):
                self.board[row][col] = random.randint(1, 6)

    def print_board(self):
        for row in self.board:
            print(" ".join(str(piece) for piece in row))

    def are_adjacent(self, row1, col1, row2, col2):
        return abs(row1 - row2) + abs(col1 - col2) == 1

    def get_matches(self, row, col):
        """Returns list of positions that match with the given position"""
        matches = [(row, col)]
        value = self.board[row][col]
        

        i = col - 1
        while i >= 0 and self.board[row][i] == value:
            matches.append((row, i))
            i -= 1
        # Check right
        i = col + 1
        while i < self.BOARD_SIZE and self.board[row][i] == value:
            matches.append((row, i))
            i += 1
            
        if len(matches) >= 3:
            return matches

        matches = [(row, col)]
        i = row - 1
        while i >= 0 and self.board[i][col] == value:
            matches.append((i, col))
            i -= 1
        i = row + 1
        while i < self.BOARD_SIZE and self.board[i][col] == value:
            matches.append((i, col))
            i += 1
            
        if len(matches) >= 3:
            return matches
            
        return []

    def clear_matches_and_refill(self, row1, col1, row2, col2):
        matches_found = False
        while True:  
            matches = set()
            if row1 is not None:  
                for row, col in [(row1, col1), (row2, col2)]:
                    matches.update(self.get_matches(row, col))
            else: 
                matches = self.find_all_matches()


            if not matches and matches_found:
                return True
  
            if not matches and not matches_found:
                return False

            matches_found = True
            
            # Clear matches
            for row, col in matches:
                self.board[row][col] = 0
            
            print("\nClearing matches:")
            self.print_board()
            time.sleep(0.5) 

    
            for col in range(self.BOARD_SIZE):
                empty_row = self.BOARD_SIZE - 1
                for row in range(self.BOARD_SIZE - 1, -1, -1):
                    if self.board[row][col] != 0:
                        if empty_row != row:
                            self.board[empty_row][col] = self.board[row][col]
                            self.board[row][col] = 0
                        empty_row -= 1
                
      
                for row in range(empty_row + 1):
                    self.board[row][col] = random.randint(1, 6)
            
            print("\nAfter falling and refilling:")
            self.print_board()
            time.sleep(0.5)  

         
            row1 = row2 = col1 = col2 = None

    def find_all_matches(self):
        """Check the entire board for matches"""
        all_matches = set()
        for row in range(self.BOARD_SIZE):
            for col in range(self.BOARD_SIZE):
                matches = self.get_matches(row, col)
                all_matches.update(matches)
        return all_matches

    def try_swap(self, row1, col1, row2, col2):
  
        if not self.are_adjacent(row1, col1, row2, col2):
            return False


        self.board[row1][col1], self.board[row2][col2] = self.board[row2][col2], self.board[row1][col1]

    
        if self.clear_matches_and_refill(row1, col1, row2, col2):
            return True


        self.board[row1][col1], self.board[row2][col2] = self.board[row2][col2], self.board[row1][col1]
        return False

def main():
    game = Match3Game()
    print("Welcome to Match 3!")
    print("Initial board:")
    game.print_board()

    while True:
        print("\nEnter swap coordinates (row1 col1 row2 col2), or 'q' to quit:")
        user_input = input()
        
        if user_input.lower() == 'q':
            break

        try:
            row1, col1, row2, col2 = map(int, user_input.split())
            if game.try_swap(row1, col1, row2, col2):
                print("Valid move! Board after swap and clearing matches:")
                game.print_board()
            else:
                print("Invalid move! Try again.")
        except ValueError:
            print("Invalid input! Please enter four numbers separated by spaces.")

if __name__ == "__main__":
    main() 